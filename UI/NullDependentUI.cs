using UnityEngine;

/// <summary>
/// NULL 오브젝트 패턴을 구현을 위한 NULL 객체
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
