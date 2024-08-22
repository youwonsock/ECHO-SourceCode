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
/// SoundManager Ŭ����
/// 
/// YWS : 2024.07.25
/// </summary>
public class SoundManager
{
    private Transform transform;
    private AudioMixer audioMixer;
    private AudioSource BGMAudioSource;

    private Queue<AudioSource> SFXAudioSourceQueue; // ������� �ƴ� AudioSource�� ��Ȱ���ϱ� ���� Queue
    private Dictionary<string, AudioClip> audioClipDict = new Dictionary<string, AudioClip>(); // AudioClip�� �����ϱ� ���� Dictionary

    private CancellationTokenSource disableCancletoken;

    //temp
    private float minDistance = 10;
    private float maxDistance = 50;


    /// <summary>
    /// AudioClip�� �������ų� ������ �߰��ϴ� �Լ�
    /// </summary>
    /// <param name="path">Load�� ����� ���</param>
    /// <param name="type">Type</param>
    /// <returns></returns>
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

    /// <summary>
    /// AudioSource ������Ʈ�� �����ϴ� �Լ�
    /// </summary>
    /// <param name="name">������ ������Ʈ�� �̸�</param>
    /// <param name="source"></param>
    private void CreateAudioSourceObject(string name, out AudioSource source)
    {
        GameObject go = new GameObject { name = name };
        source = go.AddComponent<AudioSource>();
        source.outputAudioMixerGroup = audioMixer.FindMatchingGroups(name)[0];
        source.playOnAwake = false;

        go.transform.parent = transform;
    }

    /// <summary>
    /// SFX�� AudioSource ������Ʈ�� �����ϴ� �Լ�
    /// </summary>
    /// <param name="sfx"></param>
    private void CreateSFXAudioSourceObject(out AudioSource sfx)
    {
        CreateAudioSourceObject("SFX", out sfx);

        sfx.spatialBlend = 1;
        sfx.rolloffMode = AudioRolloffMode.Linear;
        sfx.minDistance = minDistance;
        sfx.maxDistance = maxDistance;

        SFXAudioSourceQueue.Enqueue(sfx);
    }

    /// <summary>
    /// SFX�� AudioSource ������Ʈ�� ��ȯ�ϴ� �Լ�
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    private async UniTaskVoid ReturnSFX(AudioSource source)
    {
        await UniTask.WaitUntil(() => source.isPlaying == false, PlayerLoopTiming.FixedUpdate, disableCancletoken.Token);

        source.spatialBlend = 1;
        SFXAudioSourceQueue.Enqueue(source);
    }

    /// <summary>
    /// �ӽ� SFX�� AudioSource ������Ʈ�� �����ϴ� �Լ�
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    private async UniTaskVoid DestroyTempSFX(AudioSource source)
    {
        await UniTask.WaitUntil(() => source.isPlaying == false, PlayerLoopTiming.FixedUpdate, disableCancletoken.Token);

        GameObject.Destroy(source.gameObject);
    }

    /// <summary>
    /// �ʱ�ȭ
    /// </summary>
    /// <param name="transform"></param>
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

        // SFX�� AudioSource ������Ʈ�� �̸� ����
        for (int i = 0; i < 5; i++)
        {
            AudioSource sfx;
            CreateSFXAudioSourceObject(out sfx);
        }

        // BGM�� AudioSource ������Ʈ�� ����
        CreateAudioSourceObject("BGM", out BGMAudioSource);
        BGMAudioSource.loop = true;
        BGMAudioSource.spatialBlend = 0;
    }

    /// <summary>
    /// ���� �Լ�
    /// </summary>
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

    /// <summary>
    /// 2D Sound ���
    /// </summary>
    /// <param name="path">Clip�� ���� ��� Load�� ����� ���</param>
    /// <param name="type">Type</param>
    /// <param name="volume"></param>
    /// <param name="pitch"></param>
    public void PlaySound2D(string path, SoundType type, float volume = 1, float pitch = 1.0f)
    {
        AudioClip audioclip = GetOrAddAudioClip(path, type);
        PlaySound2D(audioclip, type, volume, pitch);
    }

    /// <summary>
    /// 2D Sound ���
    /// </summary>
    /// <param name="audioClip">clip</param>
    /// <param name="type">Type</param>
    /// <param name="volume"></param>
    /// <param name="pitch"></param>
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

    /// <summary>
    /// 3D Sound ���
    /// </summary>
    /// <param name="path">Clip�� ���� ��� Load�� ����� ���</param>
    /// <param name="transform">��� ��ġ</param>
    /// <param name="type">Type</param>
    /// <param name="volume"></param>
    /// <param name="pitch"></param>
    public void PlaySound3D(string path, Transform transform, SoundType type = SoundType.SFX, float volume = 1, float pitch = 1.0f)
    {
        AudioClip audioclip = GetOrAddAudioClip(path, type);
        PlaySound3D(audioclip, transform, type, volume, pitch);
    }

    /// <summary>
    /// 3D Sound ���
    /// </summary>
    /// <param name="audioClip">����� Clip</param>
    /// <param name="transform">��� ��ġ</param>
    /// <param name="type">Type</param>
    /// <param name="volume"></param>
    /// <param name="pitch"></param>
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
