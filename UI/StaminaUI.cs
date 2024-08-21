using UnityEngine;
using UnityEngine.UI;

public class StaminaUI : MonoBehaviour, IUIBase, IProgressUIView
{
    [SerializeField] private Image fillImage;

    public void GetRectTransform(out RectTransform rt)
    {
        TryGetComponent<RectTransform>(out rt);
    }

    public UIType GetUIType()
    {
        return UIType.Scene;
    }

    public void UpdateUI(float value)
    {
        fillImage.fillAmount = value;
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

    public void SetValue(float value)
    {
        fillImage.fillAmount = value;
    }
}
