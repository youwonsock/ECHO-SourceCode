using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

/// <summary>
/// 모든 Scene의 base 클래스
/// 
/// YWS : 2024.07.05
/// </summary>
public abstract class BaseScene : MonoBehaviour
{
    [SerializeField] private SceneType type;
    public SceneType Type { get { return type; } protected set { type = value; } }



    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;

        Release();
    }

    protected virtual void Awake()
    {
        Object obj = GameObject.FindObjectOfType(typeof(EventSystem));

        if (obj == null)
            Instantiate<GameObject>(Resources.Load<GameObject>("Prefab/EventSystem"));

        Init();
    }



    protected abstract void OnSceneLoaded(Scene scene, LoadSceneMode mode);

    // Awake에서 호출되는 초기화 메서드
    protected virtual void Init() { }

    // OnDisable에서 호출되는 해제 메서드
    protected virtual void Release() { }
}
