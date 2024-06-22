using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [Header("Menu Fields")] public GameObject Base;
    public GameObject Root;
    public GameObject Settings;
    public GameObject Controls;

    [Header("Settings Sliders")] public Slider Volume;
    public Slider Contrast;
    public Slider Brightness;

    [Space] [Header("Requirements")] public AudioMixer masterVolumeMixer;
    
    public PostProcessProfile postProcessingProfile;
    private AutoExposure exposure;
    private ColorGrading colorGrading;

    [Header("Misc.")] public GameObject currentScreen;
    
    void Start()
    {
        DontDestroyOnLoad(gameObject);

        postProcessingProfile.TryGetSettings(out exposure);
        postProcessingProfile.TryGetSettings(out colorGrading);

        float volume;
        masterVolumeMixer.GetFloat("MasterVol", out volume);
        Volume.value = volume;
        
        Contrast.value = colorGrading.contrast.value;
        Brightness.value = exposure.keyValue.value;
    }

    public void SetCurrentScreen(GameObject newScreen)
    {
        currentScreen = newScreen;
    }

    public void SetVisibility(bool value)
    {
        if (Base.activeSelf == value)
            return;
        
        if (Base.activeSelf)
        {
            Settings.SetActive(false);
            Controls.SetActive(false);
        }

        Base.SetActive(value);
        Root.SetActive(value);
        currentScreen = Root;
    }

    public void OnBackPressed()
    {
        if(Base.activeSelf && currentScreen.GetComponent<SettingsScreen>().previousPage != null)
            currentScreen.GetComponent<SettingsScreen>().Back();
    }

    public void OnVolumeChange(System.Single value)
    {
        masterVolumeMixer.SetFloat("MasterVol", value);
    }

    public void OnBrightnessChange(System.Single value)
    {
        exposure.keyValue.value = value;
    }
    
    public void OnContrastChange(System.Single value)
    {
        colorGrading.contrast.value = value;
    }

}
