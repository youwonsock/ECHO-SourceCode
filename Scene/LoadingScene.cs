using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScene : BaseScene
{
    protected override void Init()
    {
        Debug.Log("LoadingScene Init");
    }

    protected override void Release()
    {
        Debug.Log("LoadingScene Release");
    }

    protected override void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("LoadingScene Loaded");

        // �ϴ� YWS TestScene�� �̵�
        //StartCoroutine(LoadNext());
    }

    IEnumerator LoadNext()
    {
        yield return new WaitForSeconds(5.0f);

        GameManager.Instance.SceneManager.LoadScene(SceneType.MAX);
    }
}
