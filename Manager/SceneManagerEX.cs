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
/// 비동기 Scene 로딩 기능의 편리한 사용을 위한 SceneManager Wrapper 클래스
/// 
/// YWS : 2024.07.05
/// </summary>
public class SceneManagerEX 
{
    bool isLoadScene = false;


    /// <summary>
    /// 비동기 Scene 로딩 함수
    /// </summary>
    /// <param name="type">로딩할 Scene의 Type</param>
    /// <returns></returns>
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
            if(ao.progress >= 0.9f)
                ao.allowSceneActivation = true;

            await UniTask.Yield();
        }

        isLoadScene = false;
    }

    /// <summary>
    /// 현제 활성화된 Scene의 이름을 반환
    /// </summary>
    /// <returns></returns>
    public string GetSceneName()
    {
        return Enum.GetName(typeof(SceneType), SceneManager.GetActiveScene().buildIndex);
    }

    /// <summary>
    /// 현제 활성화된 Scene의 Type을 반환
    /// </summary>
    /// <returns></returns>
    public SceneType GetSceneType()
    {
        return (SceneType)SceneManager.GetActiveScene().buildIndex;
    }

    /// <summary>
    /// Scene 로딩 함수
    /// </summary>
    /// <param name="type"></param>
    public void LoadScene(SceneType type)
    {
        if (isLoadScene)
            return;
        
        // 비동기 로딩
        isLoadScene = true;
        LoadSceneAsync(type).Forget();
    }
}
