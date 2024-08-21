using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QTEUI : MonoBehaviour, IUIBase, IUpdateable, IProgressUIView
{
    [SerializeField] private Image image;
    [SerializeField] private List<Sprite> buttonIMG = new List<Sprite>();
    [SerializeField] private Image fillImage;

    private float time = 0.0f;
    int idx = 0;

    private void OnEnable()
    {
        UpdateManager.OnSubscribe(this, false, true, false);
    }

    private void OnDisable()
    {
        UpdateManager.OffSubscribe(this, false, true, false);
    }

    public void FixedUpdateWork() 
    { 
        if(time >= 0.04f)
        {
            time = 0;
            idx = ++idx % buttonIMG.Count;

            image.sprite = buttonIMG[idx];
        }

        time += Time.fixedDeltaTime;
    }
    public void UpdateWork() { }
    public void LateUpdateWork() { }

    public void UpdateUI(float value)
    {
        fillImage.fillAmount = value;
    }



    public void GetRectTransform(out RectTransform rt)
    {
        TryGetComponent<RectTransform>(out rt);
    }

    public UIType GetUIType()
    {
        return UIType.Popup;
    }

    public void Init()
    {
        GameManager.Instance.PlayerPresenter.progressUIView = this;

    }

    public void Release()
    {
        GameManager.Instance.PlayerPresenter.progressUIView = null;

        Destroy(gameObject);
    }
}

