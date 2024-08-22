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
        GameManager.Instance.PlayerPresenter.SetPlayerModel();  // MVP ������ ����ϱ����� Presenter�� Model�� ����

        GameManager.Instance.UIManager.CloaseAll(); // �� ��ȯ�� ��� UI�� ����
    }
}
