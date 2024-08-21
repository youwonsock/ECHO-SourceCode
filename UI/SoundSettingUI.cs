using System.IO;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// SoundSettingUI
/// 
/// YWS : 2024.07.11
/// </summary>
public class SoundSettingUI : MonoBehaviour, IUIBase, ISave, ISettingData
{
    private struct SettingData
    {
        public float masterVolume;
        public float bgmVolume;
        public float sfxVolume;
        public bool isMute;
    }

    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Toggle muteToggle;



    private void OnEnable()
    {
        masterSlider.onValueChanged.AddListener(SetMasterChanged);
        bgmSlider.onValueChanged.AddListener(SetBGMChanged);
        sfxSlider.onValueChanged.AddListener(SetSFXChanged);
        muteToggle.onValueChanged.AddListener(SetMute);

        GameManager.Instance.DataManager.AddSaveable("SoundSetting", this);
    }

    private void OnDisable()
    {
        masterSlider.onValueChanged.RemoveListener(SetMasterChanged);
        bgmSlider.onValueChanged.RemoveListener(SetBGMChanged);
        sfxSlider.onValueChanged.RemoveListener(SetSFXChanged);
        muteToggle.onValueChanged.RemoveListener(SetMute);

        GameManager.Instance.DataManager.RemoveSaveable("SoundSetting");
    }



    public void Init()
    {
        GameManager.Instance.DataManager.Load("SoundSetting");
    }

    public void Release()
    {
        GameManager.Instance.DataManager.Save("SoundSetting");

        Destroy(gameObject);
    }

    public UIType GetUIType()
    {
        return UIType.Dependent;
    }

    public void GetRectTransform(out RectTransform rt)
    {
        TryGetComponent<RectTransform>(out rt);
    }

    public void Save(string path)
    {
        SettingData settingData = new SettingData();
        settingData.masterVolume = masterSlider.value;
        settingData.bgmVolume = bgmSlider.value;
        settingData.sfxVolume = sfxSlider.value;
        settingData.isMute = muteToggle.isOn;

        string json = JsonUtility.ToJson(settingData);
        File.WriteAllText(path, json);
    }

    public void Load(string path)
    {
        string json = File.ReadAllText(path);
        SettingData settingData = JsonUtility.FromJson<SettingData>(json);

        masterSlider.value = settingData.masterVolume;
        bgmSlider.value = settingData.bgmVolume;
        sfxSlider.value = settingData.sfxVolume;
        muteToggle.isOn = settingData.isMute;
    }

    public void LoadSettingData()
    {
        SetMasterChanged(masterSlider.value);
        SetBGMChanged(bgmSlider.value);
        SetSFXChanged(sfxSlider.value);
        SetMute(muteToggle.isOn);
    }



    public void SetMasterChanged(float value)
    {
        GameManager.Instance.SoundManager.SetMasterVolume(value);
        
        // need to save data
    }

    public void SetBGMChanged(float value)
    {
        GameManager.Instance.SoundManager.SetBGMVolume(value);
    }

    public void SetSFXChanged(float value)
    {
        GameManager.Instance.SoundManager.SetSFXVolume(value);
    }

    public void SetMute(bool value)
    {
        if(value)
            GameManager.Instance.SoundManager.SetMute();

        // 마지막 값으로 복귀
    }
}
