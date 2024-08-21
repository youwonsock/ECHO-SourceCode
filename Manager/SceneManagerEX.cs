using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public enum SceneType
{
    MainMenu = 0,
    Loading,
    InGame,
    MAX
}

/// <summary>
/// �񵿱� Scene �ε� ����� ���� ����� ���� SceneManager Wrapper Ŭ����
/// 
/// YWS : 2024.07.05
/// </summary>
public class SceneManagerEX 
{
    bool isLoadScene = false;



    private async UniTaskVoid LoadSceneAsync(SceneType type)
    {
        // fade out

        // load loading scene
        SceneManager.LoadScene((int)SceneType.Loading);
        await UniTask.WaitUntil(() => GetSceneType() == SceneType.Loading);

        // load target scene
        AsyncOperation ao = SceneManager.LoadSceneAsync((int)type);
        ao.allowSceneActivation = false;

        // collect garbage and sleep main thread
        GC.Collect();
        GC.WaitForPendingFinalizers();

        while (!ao.isDone)
        {
            Debug.Log("Progress : " + ao.progress * 100);

            if(ao.progress >= 0.9f)
                ao.allowSceneActivation = true;

            await UniTask.Yield();
        }
        isLoadScene = false;

        // fade in

    }


    public string GetSceneName()
    {
        return Enum.GetName(typeof(SceneType), SceneManager.GetActiveScene().buildIndex);
    }

    public SceneType GetSceneType()
    {
        return (SceneType)SceneManager.GetActiveScene().buildIndex;
    }

    public void LoadScene(SceneType type)
    {
        if (isLoadScene)
            return;
        
        // �񵿱� �ε�
        isLoadScene = true;
        LoadSceneAsync(type).Forget();
    }

    // for test scene change
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
