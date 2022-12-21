using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using TMPro;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour {
    [SerializeField]
    private LevelList levelList;
    [SerializeField]
    private AudioMixer audioMixer;
    [SerializeField]
    private float muteDB = -80;
    [SerializeField]
    private float defaultDB = 0;
    [SerializeField]
    private TMP_Dropdown resolutionDropdown;
    [SerializeField]
    private Toggle musicToggle;
    [SerializeField]
    private Toggle SFXToggle;
    [SerializeField]
    private Toggle FullscreenToggle;

    private Resolution[] resolutions;
    private MenuUI menu;
    private AudioManager audioManager;
    private JsonSaving jsonSave;

    private void Awake() {
        menu = GetComponent<MenuUI>();
        audioManager = FindObjectOfType<AudioManager>();
        jsonSave = FindObjectOfType<JsonSaving>();
        SetDropdownResolution();
    }

    private void Start() {
        LoadSettings();
    }

    private void LoadSettings() {
        bool toggle = false;

        if (PlayerPrefs.HasKey(SettingsKey.MusicKey))
        {
            toggle = PlayerPrefs.GetInt(SettingsKey.MusicKey) == 1 ? true : false;
            musicToggle.isOn = toggle;
            SetMusic(toggle);
        }
        if (PlayerPrefs.HasKey(SettingsKey.FullscreenKey))
        {
            toggle = PlayerPrefs.GetInt(SettingsKey.MusicKey) == 1 ? true : false;
            FullscreenToggle.isOn = toggle;
            SetFullscreen(toggle);
        }
        if (PlayerPrefs.HasKey(SettingsKey.ResolutionKey))
        {
            SetResolution(PlayerPrefs.GetInt(SettingsKey.ResolutionKey));
        }
        if (PlayerPrefs.HasKey(SettingsKey.GraphicKey))
        {
            SetGraphicQuality(PlayerPrefs.GetInt(SettingsKey.GraphicKey));
        }
        if (PlayerPrefs.HasKey(SettingsKey.SFXKey))
        {
            toggle = PlayerPrefs.GetInt(SettingsKey.SFXKey) == 1 ? true : false;
            SFXToggle.isOn = toggle;
            SetSFX(toggle);
        }
    }

    private void SetDropdownResolution() {
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        
        List<string> options = new List<string>();

        int currentResIndex = 0;
        int i = 0;
        foreach (Resolution res in resolutions)
        {
            options.Add($"{res.width} x {res.height}");

            if (res.width == Screen.currentResolution.width &&
                res.height == Screen.currentResolution.height)
            {
                currentResIndex = i;
            }

            i++;
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResIndex;
        resolutionDropdown.RefreshShownValue();
    }

    public void SetResolution(int resIndex) {
        audioManager.PlaySFX(audioManager.UISwitch);
        Resolution res = resolutions[resIndex];
        Screen.SetResolution(res.width, res.height, Screen.fullScreen);
        PlayerPrefs.SetInt(SettingsKey.ResolutionKey, resIndex);
    }

    public void SetMusic(bool toggle) {
        audioManager.PlaySFX(audioManager.UISwitch);
        if (toggle)
            audioMixer.SetFloat("MusicVolume", -7f);
        else
            audioMixer.SetFloat("MusicVolume", muteDB);

        PlayerPrefs.SetInt(SettingsKey.MusicKey, toggle ? 1 : 0);
    }

    public void SetSFX(bool toggle) {
        audioManager.PlaySFX(audioManager.UISwitch);
        if (toggle)
            audioMixer.SetFloat("SFXVolume", defaultDB);
        else
            audioMixer.SetFloat("SFXVolume", muteDB);

        PlayerPrefs.SetInt(SettingsKey.SFXKey, toggle ? 1 : 0);
    }

    public void SetFullscreen(bool toggle) {
        audioManager.PlaySFX(audioManager.UISwitch);
        Screen.fullScreen = toggle;
        PlayerPrefs.SetInt(SettingsKey.FullscreenKey, toggle ? 1 : 0);
    }

    public void SetGraphicQuality(int qualityIndex) {
        audioManager.PlaySFX(audioManager.UISwitch);
        QualitySettings.SetQualityLevel(qualityIndex);
        PlayerPrefs.SetInt(SettingsKey.GraphicKey, qualityIndex);
    }

    public void ResetProgress() {
        audioManager.PlaySFX(audioManager.UIClick);

        jsonSave.DeleteSaveFile();

        menu.DestroyLevelList();
        menu.GenerateLevelList();
    }
}