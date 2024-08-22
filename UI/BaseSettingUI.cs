using UnityEngine;

/// <summary>
/// SettingUI�� �⺻�� �Ǵ� UI
/// 
/// YWS : 2024.07.11
/// </summary>
public class BaseSettingUI : MonoBehaviour, IUIBase
{
    [SerializeField] private Transform PopupArea;   // DependentUI�� �Ҽӵ� �θ� Transform
    [SerializeField] private GameObject GraphicSettingUI;
    [SerializeField] private GameObject SoundSettingUI;
    [SerializeField] private GameObject KeySettingUI;

    // ������ �����Ǵ� UI�� �ƴ� BaseSettingUI�� ���� �ֱ⿡ ���ӵǴ� UI
    IUIBase currentDependentUI = new NullDependentUI();


    public void Init()
    {
        Time.timeScale = 0;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void Release()
    {
        if (currentDependentUI != null)
            currentDependentUI.Release();

        Destroy(gameObject);
    }

    public UIType GetUIType()
    {
        return UIType.Popup;
    }

    public void GetRectTransform(out RectTransform rt)
    {
        TryGetComponent<RectTransform>(out rt);
    }



    private void SetRectTransform(RectTransform rt)
    {
        rt.SetParent(PopupArea);

        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;

        rt.localScale = Vector3.one;

        rt.offsetMin = new Vector2(0, 0);
        rt.offsetMax = new Vector2(0, 0);
    }

    /// <summary>
    /// �� UI�� ���ӵǴ� UI�� �����ش�.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="go"></param>
    private void ShowDependentUI<T>(GameObject go) where T : IUIBase
    {
        currentDependentUI.Release();

        currentDependentUI = GameManager.Instance.UIManager.ShowUI<T>(go);
        currentDependentUI.GetRectTransform(out RectTransform rt);
        SetRectTransform(rt);
    }

    public void OnClickGraphicButton()
    {
        ShowDependentUI<GraphicSettingUI>(GraphicSettingUI);
    }

    public void OnClickSoundButton()
    {
        ShowDependentUI<SoundSettingUI>(SoundSettingUI);
    }

    public void OnClickGameplayButton()
    {
        ShowDependentUI<GameplaySettingUI>(KeySettingUI);
    }

    public void OnClickCloseButton()
    {
        GameManager.Instance.UIManager.CloaseCurrentUI();
    }
}
