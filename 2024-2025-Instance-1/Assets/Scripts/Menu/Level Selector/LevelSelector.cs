using UnityEngine;
using UnityEngine.SceneManagement;

namespace Menu.Level_Selector
{
    public class LevelSelector : MonoBehaviour
    {
        private LevelInfo _currentLevel;
        public static LevelSelector instance { get; private set; }

        public LevelInfo currentLevel => _currentLevel;

        private void Awake()
        {
            if (instance == null)
                instance = this;
            else
                Destroy(gameObject);
                
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            LevelInfo.completedLevels.Clear();
        }

        public void StartLevel(LevelInfo level)
        {
            if (!level)
            {
                _currentLevel = null;
                SceneManager.LoadScene(0);
            }

            _currentLevel = level;
            level.Load();

            StartCoroutine(Utils.InvokeAfterUnscaled(() => EventManager.instance.onWin.AddListener(OnWin), 1.0f));
        }

        private void OnWin()
        {
            _currentLevel.MarkComplete();
        }
    }
}