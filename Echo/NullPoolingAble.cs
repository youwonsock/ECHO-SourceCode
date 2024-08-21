using UnityEngine;


/// <summary>
/// NullObjectPoolingAble Ŭ����
/// 
/// YWS : 2024.07.25
/// </summary>
public class NullPoolingAble : MonoBehaviour, IPoolingAble
{
    public void Activate(Transform transform)
    {

    }

    public void Deactivate()
    {

    }

    public PoolingType GetPoolingType()
    {
        return PoolingType.MAX;
    }

    public bool IsActivate()
    {
        return false;
    }
}
