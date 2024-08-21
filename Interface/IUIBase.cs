using UnityEngine;

public enum UIType
{
    Scene,
    Popup,
    Dependent
}

/// <summary>
/// UIManager�� �����Ǵ� ��� UI�� ��ӹ޾ƾ� �ϴ� �������̽�
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
