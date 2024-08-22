using UnityEngine;
using UnityEngine.UI;

public class RunButton : MonoBehaviour
{
    [SerializeField]
    private Image image;

    [SerializeField]
    private Sprite runIMG;
    [SerializeField]
    private Sprite walkIMG;

    public void isRun(bool isRun)
    {
        if (isRun)
        {
            image.sprite = runIMG;
        }
        else
        {
            image.sprite = walkIMG;
        }
    }
}
