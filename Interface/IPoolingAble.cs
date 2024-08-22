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
    /// <summary>
    /// PoolingType 반환
    /// </summary>
    /// <returns>객체의 PoolingType</returns>
    public PoolingType GetPoolingType();

    /// <summary>
    /// 활성화 여부 반환
    /// </summary>
    /// <returns></returns>
    public bool IsActivate();

    /// <summary>
    /// 활성화
    /// </summary>
    /// <param name="transform"></param>
    public void Activate(Transform transform);
    
    /// <summary>
    /// 비활성화
    /// </summary>
    public void Deactivate();
}
