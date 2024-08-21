using UnityEngine;

/// <summary>
/// NULL ������Ʈ ������ ������ ���� NULL ��ü
/// 
/// YWS : 2024.07.11
/// </summary>
public class NullDependentUI : IUIBase
{
    public void GetRectTransform(out RectTransform rt)
    {
        rt = null;
    }

    public UIType GetUIType()
    {
        return UIType.Dependent;
    }

    public void Init()
    {

    }

    public void Release()
    {

    }
}
