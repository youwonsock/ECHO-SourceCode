using UnityEngine;

public enum UIType
{
    Scene,
    Popup,
    Dependent
}

/// <summary>
/// UIManager에 관리되는 모든 UI가 상속받아야 하는 인터페이스
/// 
/// YWS : 2024.07.08
/// </summary>
public interface IUIBase
{
    public void Init();

    public void Release();

    public UIType GetUIType();

    public void GetRectTransform(out RectTransform rt);
}
