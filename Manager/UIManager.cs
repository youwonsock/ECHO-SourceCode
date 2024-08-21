using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

/// <summary>
/// UI sortOrder 및 관리를 위한 클래스
/// 
/// YWS : 2024.07.17
/// </summary>
public class UIManager
{
    private int popupSortOrder = 10;

    private Stack<IUIBase> popupStack;

    private IUIBase currentSceneUI = null;
    private Transform transform;



    public void Init(Transform transform)
    {
        try
        {
            if (transform == null)
                throw new System.Exception("transform is null!");

            this.transform = transform;

            popupSortOrder = 10;
            popupStack = new Stack<IUIBase>();
        }
        catch (System.Exception e)
        {
            Debug.LogError(e.Message);

            //Application.Quit();
            return;
        }
    }

    public void Release()
    {
        CloaseAll();
    }



    private void SetCanvas(GameObject UI, bool sort = true)
    {
        UI.TryGetComponent<Canvas>(out Canvas canvas);

        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.overrideSorting = true;

        if (sort)
            canvas.sortingOrder = popupSortOrder;
        else
            canvas.sortingOrder = 0;
    }

    public IUIBase ShowUI<T>(string name) where T : IUIBase
    {
        GameObject go = Resources.Load<GameObject>(name);

        if(go == null)
        {
            Debug.LogError("UI Load Error! : " + name);
            return null;
        }

        return ShowUI<T>(go);
    }

    public IUIBase ShowUI<T>(GameObject go) where T : IUIBase
    {
        GameObject instance = GameObject.Instantiate<GameObject>(go);
        instance.TryGetComponent<T>(out T ui);

        if (ui.GetUIType() == UIType.Scene)
        {
            if (currentSceneUI != null)
                CloaseAll();

            currentSceneUI = ui;

            SetCanvas(instance, false);
        }
        else if (ui.GetUIType() == UIType.Popup)
        {
            popupStack.Push(ui);
            ++popupSortOrder;

            SetCanvas(instance);
        }

        ui.Init();
        instance.transform.SetParent(transform);

        return ui;
    }

    public void CloaseAll()
    {
        if (popupStack != null)
        {
            while (popupStack.Count > 0)
                popupStack.Pop().Release();
        }

        if (currentSceneUI != null)
        {
            currentSceneUI.Release();
            currentSceneUI = null;
        }
    }

    public void CloaseCurrentUI()
    {
        if(popupStack.Count > 0)
        {
            CloasePopup();
            return;
        }

        if (currentSceneUI != null)
        {
            currentSceneUI.Release();
            currentSceneUI = null;
        }
    }

    public void CloasePopup()
    {
        if (popupStack.Count > 0)
        {
            popupStack.Pop().Release();
            --popupSortOrder;
        }
    }
}

