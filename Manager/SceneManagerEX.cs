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


    /// <summary>
    /// �񵿱� Scene �ε� �Լ�
    /// </summary>
    /// <param name="type">�ε��� Scene�� Type</param>
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
    /// ���� Ȱ��ȭ�� Scene�� �̸��� ��ȯ
    /// </summary>
    /// <returns></returns>
    public string GetSceneName()
    {
        return Enum.GetName(typeof(SceneType), SceneManager.GetActiveScene().buildIndex);
    }

    /// <summary>
    /// ���� Ȱ��ȭ�� Scene�� Type�� ��ȯ
    /// </summary>
    /// <returns></returns>
    public SceneType GetSceneType()
    {
        return (SceneType)SceneManager.GetActiveScene().buildIndex;
    }

    /// <summary>
    /// Scene �ε� �Լ�
    /// </summary>
    /// <param name="type"></param>
    public void LoadScene(SceneType type)
    {
        if (isLoadScene)
            return;
        
        // �񵿱� �ε�
        isLoadScene = true;
        LoadSceneAsync(type).Forget();
    }
}
