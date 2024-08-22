using UnityEngine.SceneManagement;

public class InGameScene : BaseScene
{
    protected override void Init()
    {
    }

    protected override void Release()
    {
    }

    protected override void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GameManager.Instance.PlayerPresenter.SetPlayerModel();  // MVP 패턴을 사용하기위해 Presenter에 Model을 설정

        GameManager.Instance.UIManager.CloaseAll(); // 씬 전환시 모든 UI를 닫음
    }
}
