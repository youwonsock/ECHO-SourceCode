using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

/// <summary>
/// ObjectPooling Å¬·¡½º
/// 
/// YWS : 2024.07.25
/// </summary>
/// 
public class ObjectPool
{
    private Transform transform;

    private GameObject[] poolingAbleArray;
    private IPoolingAble nullPoolingAble;
    private Dictionary<PoolingType, Queue<IPoolingAble>> queueDict;

    private CancellationTokenSource disableCancletoken;



    private IPoolingAble CreatePoolingAble(PoolingType type)
    {
        if (poolingAbleArray[(int)type] != null)
        {
            GameObject newObj = GameObject.Instantiate<GameObject>(poolingAbleArray[(int)type]);

            if(newObj == null)
                return nullPoolingAble;

            newObj.SetActive(false);
            newObj.transform.SetParent(transform);

            return newObj.GetComponent<IPoolingAble>();
        }

        return null;
    }

    private async UniTaskVoid ReturnPoolingAble(IPoolingAble poolingAble)
    {
        await UniTask.WaitUntil(() => poolingAble.IsActivate() == false, PlayerLoopTiming.FixedUpdate, disableCancletoken.Token);

        if (queueDict.TryGetValue(poolingAble.GetPoolingType(), out var queue))
        {
            poolingAble.Deactivate();
            queue.Enqueue(poolingAble);
        }
    }



    public void Init(Transform transform)
    {
        try
        {
            { 
                if (queueDict == null)
                    queueDict = new Dictionary<PoolingType, Queue<IPoolingAble>>();

                if (poolingAbleArray == null)
                    poolingAbleArray = new GameObject[(int)PoolingType.MAX];

                if (disableCancletoken == null)
                    disableCancletoken = new CancellationTokenSource();

                if (nullPoolingAble == null)
                {
                    var obj = new GameObject("NullPoolingAble");
                    obj.SetActive(false);
                    obj.transform.SetParent(transform);

                    nullPoolingAble = obj.AddComponent<NullPoolingAble>();
                }
            }


            if (transform == null)
                throw new System.Exception("transform is null!");

            this.transform = transform;

            // get all pooling able objects in prefab folder
            Object[] prefabs = Resources.LoadAll("Prefab");

            foreach (Object obj in prefabs)
            {
                GameObject go = obj as GameObject;

                if (go != null)
                {
                    if (go.TryGetComponent(out IPoolingAble poolingAble))
                    {
                        PoolingType type = poolingAble.GetPoolingType();

                        if (queueDict.ContainsKey(type))
                            throw new System.Exception("PoolingType is Duplicated! : " + type);

                        queueDict.Add(type, new Queue<IPoolingAble>());
                        poolingAbleArray[(int)type] = go;

                        for (int i = 0; i < 10; ++i)
                        {
                            IPoolingAble temp = CreatePoolingAble(type);

                            if (temp == null)
                                throw new System.Exception("CreatePoolingAble() is null");

                            queueDict[type].Enqueue(temp);
                        }
                    }
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError(e.Message);

            //Application.Quit();
            return;
        }
    }

    public void Release()
    {
        disableCancletoken?.Cancel();
        disableCancletoken?.Dispose();

        queueDict?.Clear();
    }

    public IPoolingAble Get(PoolingType type)
    {
        if (queueDict.TryGetValue(type, out var queue))
        {
            IPoolingAble poolingAble;

            if (queue.Count > 0)
                poolingAble = queue.Dequeue();
            else
                poolingAble = CreatePoolingAble(type);

            return poolingAble;
        }

        return null;
    }

    public void Return(IPoolingAble poolingAble)
    {
        if (poolingAble.GetPoolingType() == PoolingType.MAX)
            return;

        ReturnPoolingAble(poolingAble).Forget();
    }
}