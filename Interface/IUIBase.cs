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
    /// <summary>
    /// UI 초기화 함수
    /// </summary>
    public void Init();

    /// <summary>
    /// UI 해제 함수
    /// </summary>
    public void Release();

    /// <summary>
    /// UI 타입 반환 함수
    /// </summary>
    /// <returns></returns>
    public UIType GetUIType();

    /// <summary>
    /// RectTransform 반환 함수
    /// </summary>
    /// <param name="rt">RectTransform</param>
    public void GetRectTransform(out RectTransform rt);
}
