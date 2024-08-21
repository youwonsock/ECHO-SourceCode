using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// KeySettingUI
/// 
/// YWS : 2024.07.11
/// </summary>
public class GameplaySettingUI : MonoBehaviour, IUIBase, ISave, ISettingData
{
    private struct SettingData
    {
        public float FOVValue;
        public float MouseSensitivityValue;
    }

    [SerializeField] private Slider FOVSlider;
    [SerializeField] private Slider MouseSensitivitySlider;



    private void OnEnable()
    {
        GameManager.Instance.DataManager.AddSaveable("GameplaySetting", this);
    }

    private void OnDisable()
    {
        GameManager.Instance.DataManager.RemoveSaveable("GameplaySetting");
    }



    public void Init()
    {
        GameManager.Instance.DataManager.Load("GameplaySetting");
    }

    public void Release()
    {

        GameManager.Instance.DataManager.Save("GameplaySetting");

        Destroy(gameObject);
    }

    public void Save(string path)
    {
        SettingData settingData = new SettingData();
        settingData.FOVValue = FOVSlider.value;
        settingData.MouseSensitivityValue = MouseSensitivitySlider.value;

        string json = JsonUtility.ToJson(settingData);
        File.WriteAllText(path, json);
    }

    public void Load(string path)
    {
        string json = File.ReadAllText(path);
        SettingData settingData = JsonUtility.FromJson<SettingData>(json);

        FOVSlider.value = settingData.FOVValue;
        MouseSensitivitySlider.value = settingData.MouseSensitivityValue;
    }

    public void LoadSettingData()
    {
        OnCameraFOVChange(FOVSlider.value);
        OnMouseSensitivityChange(MouseSensitivitySlider.value);
    }

    public void GetRectTransform(out RectTransform rt)
    {
        TryGetComponent<RectTransform>(out rt);
    }

    public UIType GetUIType()
    {
        return UIType.Dependent;
    }

    public void OnMouseSensitivityChange(float value)
    {
        GameManager.Instance.PlayerPresenter.MouseSensitivity = value;
    }

    public void OnCameraFOVChange(float value)
    {
        GameManager.Instance.PlayerPresenter.CameraFOV = value;
    }
}
