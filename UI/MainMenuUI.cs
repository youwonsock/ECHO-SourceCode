using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// GraphicSettingUI
/// 
/// YWS : 2024.07.11
/// </summary>
public class MainMenuUI : MonoBehaviour, IUIBase
{
    public void Init()
    {

    }
    public void Release()
    {
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



    public void OnClickStartButton()
    {
        GameManager.Instance.SceneManager.LoadScene("DIG_PrototypeScene");
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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && Time.timeScale == 0)
        {
            GameManager.Instance.UIManager.CloaseCurrentUI();
        }


        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;

    }
}
