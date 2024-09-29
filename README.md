# ECHO-SourceCode

## Developer Info
* 유원석(You Won Sock)
* GitHub : https://github.com/youwonsock
* Mail : qazwsx233434@gmail.com
* Download : https://store.steampowered.com/app/3132180/Echo/ 

## Our Game
![스크린샷 2024-09-28 223259](https://github.com/user-attachments/assets/099297dc-979a-4388-b759-40e1bb5abed4)


### Genres

3D Horror Puzzle Adventure

<b><h2>Platforms</h2></b>

<p>
<img src="https://upload.wikimedia.org/wikipedia/commons/c/c7/Windows_logo_-_2012.png" height="30">
</p>

<p>
<img src="https://upload.wikimedia.org/wikipedia/commons/7/7a/Android_logo_2019_%28white_wordmark%29.svg" height="30">
</p>

### Development kits

<p>
<img src="https://upload.wikimedia.org/wikipedia/commons/thumb/1/19/Unity_Technologies_logo.svg/1280px-Unity_Technologies_logo.svg.png" height="40">
</p>

<b><h2>Periods</h2></b>

* 2024-06 - 2023-08

## Contribution

### Manager
  * GameManager
  
  * SoundManager  
    Unity Audio Mixer와 연동하여 사운드 조절 기능 및 AudioSource Pool을 구현하였습니다.

    <details>
    <summary>SceneManagerEX Code</summary>
    <div markdown="1">

      ```c#
       using Cysharp.Threading.Tasks;
       using System.Collections.Generic;
       using System.Threading;
       using UnityEngine;
       using UnityEngine.Audio;
       
       public enum SoundType
       {
           BGM,
           SFX,
           MAX
       }
       
       /// <summary>
       /// SoundManager 클래스
       /// 
       /// YWS : 2024.07.25
       /// </summary>
       public class SoundManager
       {
           private Transform transform;
           private AudioMixer audioMixer;
           private AudioSource BGMAudioSource;
       
           private Queue<AudioSource> SFXAudioSourceQueue;
           private Dictionary<string, AudioClip> audioClipDict = new Dictionary<string, AudioClip>();
       
           private CancellationTokenSource disableCancletoken;
       
           //temp
           private float minDistance = 10;
           private float maxDistance = 50;
       
       
       
           private AudioClip GetOrAddAudioClip(string path, SoundType type)
           {
               if (path.Contains("Sound/") == false)
                   path = $"Sound/{path}";
       
               AudioClip audioClip = null;
       
               if (type == SoundType.BGM)
               {
                   audioClip = Resources.Load<AudioClip>(path);
               }
               else
               {
                   if (!audioClipDict.TryGetValue(path, out audioClip))
                   {
                       audioClip = Resources.Load<AudioClip>(path);
                       audioClipDict.Add(path, audioClip);
                   }
               }
       
               if (audioClip == null)
                   Debug.LogError($"AudioClip Missing : {path}");
       
               return audioClip;
           }
       
           private void CreateAudioSourceObject(string name, out AudioSource source)
           {
               GameObject go = new GameObject { name = name };
               source = go.AddComponent<AudioSource>();
               source.outputAudioMixerGroup = audioMixer.FindMatchingGroups(name)[0];
               source.playOnAwake = false;
       
               go.transform.parent = transform;
           }
       
           private void CreateSFXAudioSourceObject(out AudioSource sfx)
           {
               CreateAudioSourceObject("SFX", out sfx);
       
               sfx.spatialBlend = 1;
               sfx.rolloffMode = AudioRolloffMode.Linear;
               sfx.minDistance = minDistance;
               sfx.maxDistance = maxDistance;
       
               SFXAudioSourceQueue.Enqueue(sfx);
           }
       
           private async UniTaskVoid ReturnSFX(AudioSource source)
           {
               await UniTask.WaitUntil(() => source.isPlaying == false, PlayerLoopTiming.FixedUpdate, disableCancletoken.Token);
       
               source.spatialBlend = 1;
               SFXAudioSourceQueue.Enqueue(source);
           }
       
           private async UniTaskVoid DestroyTempSFX(AudioSource source)
           {
               await UniTask.WaitUntil(() => source.isPlaying == false, PlayerLoopTiming.FixedUpdate, disableCancletoken.Token);
       
               GameObject.Destroy(source.gameObject);
           }
       
       
           public void Init(Transform transform)
           {
               if (transform == null)
               {
                   Debug.LogError("SoundManager Init Error!");
                   return;
               }
       
               this.transform = transform;
               audioMixer = Resources.Load<AudioMixer>("Sound/AudioMixer");
       
               {
                   if (SFXAudioSourceQueue == null)
                       SFXAudioSourceQueue = new Queue<AudioSource>();
       
                   if (audioClipDict == null)
                       audioClipDict = new Dictionary<string, AudioClip>();
       
                   if (disableCancletoken == null)
                       disableCancletoken = new CancellationTokenSource();
               }
       
               for (int i = 0; i < 5; i++)
               {
                   AudioSource sfx;
                   CreateSFXAudioSourceObject(out sfx);
               }
       
               CreateAudioSourceObject("BGM", out BGMAudioSource);
               BGMAudioSource.loop = true;
               BGMAudioSource.spatialBlend = 0;
           }
       
           public void Release()
           {
               disableCancletoken?.Cancel();
               disableCancletoken?.Dispose();
       
               Clear();
           }
       
           public void Clear()
           {
               if (BGMAudioSource)
               {
                   BGMAudioSource.clip = null;
                   BGMAudioSource.Stop();
               }
       
               if (SFXAudioSourceQueue != null)
               {
                   foreach (AudioSource audioSource in SFXAudioSourceQueue)
                   {
                       audioSource.clip = null;
                       audioSource.Stop();
                   }
               }
       
               audioClipDict?.Clear();
           }
       
           public void PlaySound2D(string path, SoundType type, float volume = 1, float pitch = 1.0f)
           {
               AudioClip audioclip = GetOrAddAudioClip(path, type);
               PlaySound2D(audioclip, type, volume, pitch);
           }
       
           public void PlaySound2D(AudioClip audioClip, SoundType type, float volume = 1, float pitch = 1.0f)
           {
               if (audioClip == null)
                   return;
       
               if (type == SoundType.BGM)
               {
                   if (BGMAudioSource.isPlaying)
                       BGMAudioSource.Stop();
       
                   BGMAudioSource.pitch = pitch;
                   BGMAudioSource.clip = audioClip;
                   BGMAudioSource.Play();
               }
               else
               {
                   AudioSource audioSource;
       
                   if (!SFXAudioSourceQueue.TryDequeue(out audioSource))
                   {
                       CreateSFXAudioSourceObject(out audioSource);
                   }
       
                   audioSource.pitch = pitch;
                   audioSource.volume = volume;
                   audioSource.clip = audioClip;
                   audioSource.spatialBlend = 0;
       
                   audioSource.Play();
       
                   ReturnSFX(audioSource).Forget();
               }
           }
       
           public void PlaySound3D(string path, Transform transform, SoundType type = SoundType.SFX, float volume = 1, float pitch = 1.0f)
           {
               AudioClip audioclip = GetOrAddAudioClip(path, type);
               PlaySound3D(audioclip, transform, type, volume, pitch);
           }
       
           public void PlaySound3D(AudioClip audioClip, Transform transform, SoundType type = SoundType.SFX, float volume = 1, float pitch = 1.0f)
           {
               if (audioClip == null)
                   return;
       
               if (type == SoundType.SFX)
               {
                   AudioSource audioSource;
                   bool isTemp = false;
       
                   if (!SFXAudioSourceQueue.TryDequeue(out audioSource))
                   {
                       CreateSFXAudioSourceObject(out audioSource);
                       isTemp = true;
                   }
       
                   audioSource.pitch = pitch;
                   audioSource.volume = volume;
                   audioSource.clip = audioClip;
                   audioSource.transform.position = transform.position;
       
                   audioSource.Play();
       
                   if (isTemp)
                       DestroyTempSFX(audioSource).Forget();
                   else
                       ReturnSFX(audioSource).Forget();
               }
           }
       
           public void SetVolume(float master, float BGM, float SFX)
           {
               audioMixer.SetFloat("Master", Mathf.Log10(master) * 20);
               audioMixer.SetFloat("BGM", Mathf.Log10(BGM) * 20);
               audioMixer.SetFloat("SFX", Mathf.Log10(SFX) * 20);
           }
       
           public void SetMasterVolume(float volume)
           {
               audioMixer.SetFloat("Master", Mathf.Log10(volume) * 20);
           }
       
           public void SetBGMVolume(float volume)
           {
               audioMixer.SetFloat("BGM", Mathf.Log10(volume) * 20);
           }
       
           public void SetSFXVolume(float volume)
           {
               audioMixer.SetFloat("SFX", Mathf.Log10(volume) * 20);
           }
       
           public void SetMute()
           {
               SetVolume(0.0001f, 0.0001f, 0.0001f);
           }
       
           public void StopBGM()
           {
               BGMAudioSource.Stop();
           }
       }
      ```
      
    </div>
    </details>

    
  * DataManager
    * ISave
    * ISettingData
  
  * SceneManagerEX  
    비동기 Scene Loading 및 Scene 관리의 편의성을 위해 제작된 확장 SceneManager 클래스입니다.
  
    <details>
    <summary>SceneManagerEX Code</summary>
    <div markdown="1">

      ```c#
       using Cysharp.Threading.Tasks;
       using System;
       using UnityEngine;
       using UnityEngine.SceneManagement;
       
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



          private async UniTaskVoid LoadSceneAsync(SceneType type)
          {
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
              
              // 비동기 로딩
              isLoadScene = true;
              LoadSceneAsync(type).Forget();
          }
      }

      ```
      
    </div>
    </details>
    
  * UIManager
  
  * ObjectPool
    * IPoolingAble

### UI
  * PlayerPresenter
  * IUIBase

    ![ui](https://github.com/user-attachments/assets/539458bc-ff6b-4e6e-9ad0-a2e6ed717b24)

### Contents
  * Echo
  * NullPoolingAble

### Shader
  * NumberShader
    
    ![echo2](https://github.com/user-attachments/assets/6b051763-f68a-4d50-8b53-0218c22947e2)

    가시성을 위해 숫자 및 도형 오브젝트에는 잔상 효과를 구현하였습니다.
    강조하고 싶은 오브젝트의 Material로 사용 시 Echo 이펙트에 반응하여 오브젝트의 Vertex 단위로 강조합니다.
    
  * GroundShader
    
    ![echo1](https://github.com/user-attachments/assets/e6b74f9c-cbbd-47a6-97a4-282b92af6fed)

    시작 위치로부터 퍼져나가는 Echo 이펙트를 Unlit 쉐이더를 사용해 구현하였습니다.
    원형으로 퍼져나가는 효과를 위해 Pixel 쉐이더에서 처리합니다.

  * EchoShaderGraph
