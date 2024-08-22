using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

/// <summary>
/// GraphicSettingUI
/// 
/// YWS : 2024.07.11
/// </summary>
public class GraphicSettingUI : MonoBehaviour, IUIBase, ISave, ISettingData
{
    private struct SettingData
    {
        public int screenModeIDX;
        public int resolutionIDX;
        public int frameRateIDX;
        public int antiAliasingIDX;
        public int vSyncIDX;
    }

    [SerializeField] private TMP_Dropdown screenModeDropdown;
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    [SerializeField] private TMP_Dropdown frameRateDropdown;
    [SerializeField] private TMP_Dropdown vSyncDropBox;
    [SerializeField] private TMP_Dropdown antiAliasingDropBox;

#if !UNITY_ANDROID
    // PC 용 프레임 레이트 Dict
    [Space(25)]private readonly static Dictionary<string, int> frameRateDict 
        = new Dictionary<string, int>() 
        {
            { "60", 60 },
            { "144", 144 },
            { "240", 240 },
            { "Unlimited", -1 }
        };

    // PC 용 해상도 리스트
    private List<Resolution> resolutions = new List<Resolution>();
    private FullScreenMode currentScreenMode = FullScreenMode.FullScreenWindow;
    private int currentResolutionIndex = 0;
#endif

    private void OnEnable()
    {

#if UNITY_ANDROID
        screenModeDropdown.transform.parent.gameObject.SetActive(false);
        resolutionDropdown.transform.parent.gameObject.SetActive(false);
        frameRateDropdown.transform.parent.gameObject.SetActive(false);
        vSyncDropBox.transform.parent.gameObject.SetActive(false);
#endif

#if !UNITY_ANDROID
        screenModeDropdown.onValueChanged.AddListener(SetScreenMode);
        resolutionDropdown.onValueChanged.AddListener(SetResolution);
        frameRateDropdown.onValueChanged.AddListener(SetFrameRate);
        vSyncDropBox.onValueChanged.AddListener(SetVSync);
#endif

        antiAliasingDropBox.onValueChanged.AddListener(SetAntiAliasing);

        GameManager.Instance.DataManager.AddSaveable("GraphicSetting", this);
    
    }

    private void OnDisable()
    {
#if !UNITY_ANDROID
        screenModeDropdown.onValueChanged.RemoveListener(SetScreenMode);
        resolutionDropdown.onValueChanged.RemoveListener(SetResolution);
        frameRateDropdown.onValueChanged.RemoveListener(SetFrameRate);
        vSyncDropBox.onValueChanged.RemoveListener(SetVSync);
#endif
        antiAliasingDropBox.onValueChanged.RemoveListener(SetAntiAliasing);
    
        GameManager.Instance.DataManager.RemoveSaveable("GraphicSetting");
    }



    public void Init()
    {
#if !UNITY_ANDROID
        GetSupportResolutions();
        SetFrameRateDropdown();
#endif

        GameManager.Instance.DataManager.Load("GraphicSetting");
    }

    public void Release()
    {
        GameManager.Instance.DataManager.Save("GraphicSetting");

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

#if !UNITY_ANDROID
        settingData.screenModeIDX = screenModeDropdown.value;
        settingData.resolutionIDX = resolutionDropdown.value;
        settingData.frameRateIDX = frameRateDropdown.value;
        settingData.vSyncIDX = vSyncDropBox.value;
#endif
        settingData.antiAliasingIDX = antiAliasingDropBox.value;

        string json = JsonUtility.ToJson(settingData);
        File.WriteAllText(path, json);
    }

    public void Load(string path)
    {
        string json = File.ReadAllText(path);
        SettingData settingData = JsonUtility.FromJson<SettingData>(json);

#if !UNITY_ANDROID
        screenModeDropdown.value = settingData.screenModeIDX;
        resolutionDropdown.value = settingData.resolutionIDX;
        frameRateDropdown.value = settingData.frameRateIDX;
        vSyncDropBox.value = settingData.vSyncIDX;
#endif

        antiAliasingDropBox.value = settingData.antiAliasingIDX;
    }

    public void LoadSettingData()
    {
#if !UNITY_ANDROID
        SetScreenMode(screenModeDropdown.value);
        SetResolution(resolutionDropdown.value);
        SetFrameRate(frameRateDropdown.value);
        SetVSync(vSyncDropBox.value);
#endif

        SetAntiAliasing(antiAliasingDropBox.value);

        // set default value
        QualitySettings.globalTextureMipmapLimit = 3;
        QualitySettings.shadows = ShadowQuality.Disable;
        QualitySettings.shadowResolution = ShadowResolution.Low;
        QualitySettings.anisotropicFiltering = AnisotropicFiltering.Disable;

        Application.targetFrameRate = 30;
    }



#if !UNITY_ANDROID

    /// <summary>
    /// 모니터가 지원하는 해상도를 가져온다.
    /// </summary>
    private void GetSupportResolutions()
    {
        Resolution[] Resolutions;

        Resolutions = Screen.resolutions;

        SortedSet<string> resolutionSet = new SortedSet<string>();
        foreach (Resolution resolution in Resolutions)
        {
            int MAX = 1;
            int width = resolution.width;
            int height = resolution.height;
            
            while(height != 0)
            {
                MAX = width % height;
                width = height;
                height = MAX;
            }
            if(MAX == 0)
                MAX = width;

            if ((resolution.width / MAX == 16) && (resolution.height / MAX == 9))   // 16:9 비율만 가져온다.
            {
                var option = new TMP_Dropdown.OptionData() { text = resolution.width + " x " + resolution.height };
                
                if(resolutionSet.Contains(option.text))
                    continue;
                resolutionSet.Add(option.text);

                resolutionDropdown.options.Add(option);
                resolutions.Add(resolution);
            }
        }
    }

    private void SetFrameRateDropdown()
    {
        foreach (var item in frameRateDict)
            frameRateDropdown.options.Add(new TMP_Dropdown.OptionData(item.Key));
    }

    public void SetScreenMode(int value)
    {
        switch (value)
        {
            case 0:
                currentScreenMode = FullScreenMode.FullScreenWindow;
                break;
            case 1:
                currentScreenMode = FullScreenMode.Windowed;
                break;
            default:
                currentScreenMode = FullScreenMode.FullScreenWindow;
                break;
        }

        Screen.SetResolution(resolutions[currentResolutionIndex].width, resolutions[currentResolutionIndex].height, currentScreenMode);
    }

    public void SetResolution(int value)
    {

        currentResolutionIndex = value;

        Screen.SetResolution(resolutions[currentResolutionIndex].width, resolutions[currentResolutionIndex].height, currentScreenMode);
    }

    public void SetFrameRate(int value)
    {
        if(frameRateDict.TryGetValue(frameRateDropdown.options[value].text.ToString(), out int frameRate))
            Application.targetFrameRate = frameRate;
    }
#endif

    /// <summary>
    /// 안티 앨리어싱 설정
    /// </summary>
    /// <param name="value"></param>
    public void SetAntiAliasing(int value)
    {
        switch (value)
        {
            case 0:
                QualitySettings.antiAliasing = 0;   // Off
                break;
            case 1:
                QualitySettings.antiAliasing = 2;   // 2x
                break;
            case 2:
                QualitySettings.antiAliasing = 4;   // 4x
                break;
            case 3:
                QualitySettings.antiAliasing = 8;   // 8x 
                break;
        }
    }

    public void SetVSync(int value)
    {
        QualitySettings.vSyncCount = value;
    }
}
