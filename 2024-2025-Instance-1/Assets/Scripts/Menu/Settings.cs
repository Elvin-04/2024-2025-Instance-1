using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown graphicsDropdown;
    [SerializeField] private Slider masterVol, musicVol, sfxVol;
    [SerializeField] private AudioMixer mainAudioMixer;
    [SerializeField] private GameObject panelSettings;
    public void ChangeGraphicsQuality()
    {
        QualitySettings.SetQualityLevel(graphicsDropdown.value);
    }

    public void ChangeMasterVolume()
    {
        mainAudioMixer.SetFloat("Master", Mathf.Log10(masterVol.value) * 20);
    }
    public void ChangeMusicVolume()
    {
        mainAudioMixer.SetFloat("Music", Mathf.Log10(musicVol.value) * 20);
    }
    public void ChangeSFXVolume()
    {
        mainAudioMixer.SetFloat("SFX", Mathf.Log10(sfxVol.value) * 20);
    }

    public void ClosePanel()
    {
        gameObject.SetActive(false);
    }
}