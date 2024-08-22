using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

/// <summary>
/// Echo Effect Scaling Ŭ����
/// 
/// YWS : 2024.07.01
/// </summary>
public class Echo : MonoBehaviour, IUpdateable, IPoolingAble
{
    [SerializeField] private SO_EchoScalingData scalingData;
    private float time = 0;
    private int echoID = 0;
    private int type = 0;


    private void OnEnable()
    {
        UpdateManager.OnSubscribe(this, false, true, false);

        echoID = GameManager.Instance.GetEchoID();
        transform.localScale = Vector3.zero;
        time = 0;

        if (scalingData.Type == PoolingType.PlayerRunEcho || scalingData.Type == PoolingType.PlayerEcho)
            type = 1;
        else
            type = 0;
    }

    private void OnDisable()
    {
        UpdateManager.OffSubscribe(this, false, true, false);
    }

    public void UpdateWork(){ }

    public void LateUpdateWork(){}

    public void FixedUpdateWork()
    {
        // �ð��� ���� ũ�� ����
        time += Time.fixedDeltaTime * scalingData.InverseScalingTime;
        float scale = Mathf.Lerp(0, scalingData.MaxScale, time);

        // gameManager�� Echo�� �����ϱ�
        GameManager.Instance.UpdateEchoRadiusAndPosition(echoID, scale, new Vector4(transform.position.x, transform.position.y, transform.position.z, type));
    }



    /// <summary>
    /// Echo ��Ȱ��ȭ ó��
    /// </summary>
    /// <returns></returns>
    private async UniTaskVoid Disable()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(scalingData.ScalingTime));

        GameManager.Instance.UpdateEchoRadiusAndPosition(echoID, 0, new Vector4(transform.position.x, transform.position.y, transform.position.z, type));

        gameObject.SetActive(false);
    }

    public PoolingType GetPoolingType()
    {
        return scalingData.Type;
    }

    public bool IsActivate()
    {
        return gameObject.activeSelf;
    }

    public void Activate(Transform transform)
    {
        this.transform.position = transform.position;
        gameObject.SetActive(true);

        // unitask �� ����� ��Ȱ��ȭ ó��
        Disable().Forget();
    }

    public void Deactivate() 
    {
        gameObject.SetActive(false);
    }
}
