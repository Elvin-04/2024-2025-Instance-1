using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace Menu
{
    public class Settings : MonoBehaviour
    {
        [SerializeField] private TMP_Dropdown _graphicsDropdown;
        [SerializeField] private Slider _masterVol;
        [SerializeField] private Slider _musicVol;
        [SerializeField] private Slider _sfxVol;
        [SerializeField] private AudioMixer _mainAudioMixer;
        [SerializeField] private GameObject _panelSettings;

        private void Start()
        {
            Assert.IsNotNull(_graphicsDropdown, "graphics dropdown is null in Settings");
            Assert.IsNotNull(_masterVol, "master volume slider is null in Settings");
            Assert.IsNotNull(_musicVol, "music volume slider is null in Settings");
            Assert.IsNotNull(_sfxVol, "sfx volume slider is null in Settings");
            Assert.IsNotNull(_mainAudioMixer, "main audio mixer is null in Settings");
            Assert.IsNotNull(_panelSettings, "panel settings is null in Settings");
        }

        public void ChangeGraphicsQuality()
        {
            QualitySettings.SetQualityLevel(_graphicsDropdown.value);
        }

        public void ChangeMasterVolume()
        {
            _mainAudioMixer.SetFloat("Master", Mathf.Log10(_masterVol.value) * 20);
        }

        public void ChangeMusicVolume()
        {
            _mainAudioMixer.SetFloat("Music", Mathf.Log10(_musicVol.value) * 20);
        }

        public void ChangeSfxVolume()
        {
            _mainAudioMixer.SetFloat("SFX", Mathf.Log10(_sfxVol.value) * 20);
        }

        public void ClosePanel()
        {
            gameObject.SetActive(false);
        }
    }
}