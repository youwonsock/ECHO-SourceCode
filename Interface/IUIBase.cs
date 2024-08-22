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
    /// <summary>
    /// UI �ʱ�ȭ �Լ�
    /// </summary>
    public void Init();

    /// <summary>
    /// UI ���� �Լ�
    /// </summary>
    public void Release();

    /// <summary>
    /// UI Ÿ�� ��ȯ �Լ�
    /// </summary>
    /// <returns></returns>
    public UIType GetUIType();

    /// <summary>
    /// RectTransform ��ȯ �Լ�
    /// </summary>
    /// <param name="rt">RectTransform</param>
    public void GetRectTransform(out RectTransform rt);
}
