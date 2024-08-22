using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Singleton Å¬·¡½º
/// 
/// YWS : 2024.06.16
/// </summary>
public class Singleton<T> : SerializedMonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    private static object _lock = new object();



    private void Awake()
    {
        if (instance == null)
        {
            instance = this as T;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            // already exist, destroy self
            Destroy(this.gameObject);
        }

        Init();
    }

    private void OnDestroy()
    {
        if (instance == this)
            instance = null;
    }



    protected virtual void Init()
    {
        // override this method for initialization
    }

    public static T Instance
    {
        get
        {
            lock (_lock)
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<T>();

                    if (instance == null)
                    {
                        GameObject obj = new GameObject();
                        obj.name = typeof(T).Name;
                        instance = obj.AddComponent<T>();

                        DontDestroyOnLoad(obj);
                    }
                }

                return instance;
            }
        }
    }
}
