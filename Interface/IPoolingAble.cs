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
    /// <summary>
    /// PoolingType ��ȯ
    /// </summary>
    /// <returns>��ü�� PoolingType</returns>
    public PoolingType GetPoolingType();

    /// <summary>
    /// Ȱ��ȭ ���� ��ȯ
    /// </summary>
    /// <returns></returns>
    public bool IsActivate();

    /// <summary>
    /// Ȱ��ȭ
    /// </summary>
    /// <param name="transform"></param>
    public void Activate(Transform transform);
    
    /// <summary>
    /// ��Ȱ��ȭ
    /// </summary>
    public void Deactivate();
}
