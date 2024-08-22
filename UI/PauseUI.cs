using UnityEngine;

/// <summary>
/// Pauase UI
/// 
/// YWS : 2024.07.17
/// </summary>
public class PauseUI : MonoBehaviour, IUIBase
{
    public void Init()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;

        Time.timeScale = 0;
    }
    public void Release()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        Time.timeScale = 1;

        Destroy(gameObject);
    }

    public UIType GetUIType()
    {
        return UIType.Scene;
    }

    public void GetRectTransform(out RectTransform rt)
    {
        TryGetComponent<RectTransform>(out rt);
    }



    public void OnClickSettingButton()
    {
        GameManager.Instance.UIManager.ShowUI<BaseSettingUI>("UI/Base Setting UI");
    }

    public void OnClickExitButton()
    {
        //Debug.Log("Exit Game");
        Application.Quit();

#if !UNITY_EDITOR

        System.Diagnostics.Process.GetCurrentProcess().Kill();

#endif
    }

    public void OnClickCloaseButton()
    {
        GameManager.Instance.UIManager.CloaseCurrentUI();
    }
}
