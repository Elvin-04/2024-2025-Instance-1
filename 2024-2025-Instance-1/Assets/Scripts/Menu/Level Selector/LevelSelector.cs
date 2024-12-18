using UnityEngine;
using UnityEngine.SceneManagement;

namespace Menu.Level_Selector
{
    public class LevelSelector : MonoBehaviour
    {
        private LevelInfo _currentLevel;
        public static LevelSelector instance { get; private set; }

        public LevelInfo currentLevel => _currentLevel;

        private int _currentLevelStars = 0;

        private void Awake()
        {
            if (instance)
            {
                Destroy(gameObject);
                return;
            }

            instance = this;
            DontDestroyOnLoad(gameObject);
            LevelInfo.completedLevels.Clear();
        }

        public void StartLevel(LevelInfo level)
        {
            if (!level)
            {
                _currentLevel = null;
                _currentLevelStars = 0;
                SceneManager.LoadScene(0);
                return;
            }

            _currentLevel = level;
            level.Load();

            StartCoroutine(Utils.InvokeAfterUnscaled(AddStuffToEventManager, 1.0f));
        }

        private void OnWin()
        {
            _currentLevel.MarkComplete(_currentLevelStars);
        }

        private void AddStuffToEventManager()
        {
            EventManager.instance.onWin.AddListener(OnWin);
            EventManager.instance.onScoreUpdated.AddListener(stars => _currentLevelStars = stars);
        }
    }
}