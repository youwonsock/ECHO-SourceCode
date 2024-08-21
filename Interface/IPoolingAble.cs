using UnityEngine;

public enum PoolingType
{
    NPCEcho = 0,
    PlayerEcho = 1,
    NPCRunEcho,
    PlayerRunEcho,
    MAX
}

/// <summary>
/// ObjectPooling�� �ϴ� ��ü�� �����ؾ� �ϴ� �������̽�
/// 
/// YWS : 2024.07.04
/// </summary>
public interface IPoolingAble
{
    public PoolingType GetPoolingType();

    public bool IsActivate();

    public void Activate(Transform transform);
    public void Deactivate();
}
