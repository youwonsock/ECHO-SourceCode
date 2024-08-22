using UnityEngine;

/// <summary>
/// ����� �ػ� ������ ���� ���͹ڽ� Ŭ����
/// 
/// YWS : 2024.08.06
/// </summary>
public class LetterBox : MonoBehaviour
{

#if UNITY_ANDROID
    
    private void Awake()
    {
        TryGetComponent<Camera>(out Camera camera);
        Rect rect = camera.rect;

        float scaleheight = ((float)Screen.width / Screen.height) / ((float)16 / 9);
        float scalewidth = 1f / scaleheight;

        if (scaleheight < 1)
        {
            rect.height = scaleheight;
            rect.y = (1f - scaleheight) / 2f;
        }
        else
        {
            rect.width = scalewidth;
            rect.x = (1f - scalewidth) / 2f;
        }
    
        camera.rect = rect;
    }

#endif

}
