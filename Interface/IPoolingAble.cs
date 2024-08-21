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
/// ObjectPooling을 하는 객체가 구현해야 하는 인터페이스
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
