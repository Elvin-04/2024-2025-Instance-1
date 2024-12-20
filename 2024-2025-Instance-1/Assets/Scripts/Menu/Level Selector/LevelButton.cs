using Managers.Audio;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Menu.Level_Selector
{
    [RequireComponent(typeof(Button))]
    public class LevelButton : MonoBehaviour
    {
        [SerializeField] private LevelInfo _level;
        [SerializeField] private TMP_Text _btnText;

        private Button _btn;
        private readonly SoundsName _musicGame = SoundsName.MusicInGame;

        private void Start()
        {
            _btn = GetComponent<Button>();

            Assert.IsNotNull(_level, "level is null in LevelButton");
            Assert.IsNotNull(_btnText, "button text is null");

            _btnText.text = _level.levelName;

            if (_btn.interactable = _level.IsUnlocked)
            {
                //if (LevelInfo.completedLevels.TryGetValue(_level.levelScene, out int stars))
                //{
                //    for (int i = 0; i < stars; i++)
                //    {   
                //        GameObject starObject = Instantiate(_starPrefab);
                //        starObject.SetActive(true);
                //        starObject.transform.SetParent(_starsContainer.transform, false);
                //    }
                //}
            }
        }

        public void LoadLevel()
        {
            EventManager.instance.onPlayMusic.Invoke(_musicGame);
            SceneManager.LoadScene(_level.levelScene);
        }
    }
}