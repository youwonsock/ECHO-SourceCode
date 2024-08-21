using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameScene : BaseScene
{
    protected override void Init()
    {
        //Debug.Log("InGameScene Init");
    }

    protected override void Release()
    {
        //Debug.Log("InGameScene Release");
    }

    private void Start()
    {
        // for test   
       // GameManager.Instance.PlayerPresenter.SetPlayerModel();
    }

    protected override void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //Debug.Log("InGameScene Loaded");

        GameManager.Instance.PlayerPresenter.SetPlayerModel();
    }
}
