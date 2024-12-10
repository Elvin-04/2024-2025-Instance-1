using UnityEngine;
using UnityEngine.SceneManagement;

namespace Menu.Level_Selector
{
    public class LevelSelector : MonoBehaviour
    {
        private LevelInfo _currentLevel;
        public static LevelSelector instance { get; private set; }

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
            _currentLevel = level;
            SceneManager.LoadScene(level.levelScene);

            StartCoroutine(Utils.InvokeAfterUnscaled(() => EventManager.instance.onWin.AddListener(OnWin), 1.0f));
        }

        private void OnWin()
        {
            _currentLevel.MarkComplete();
        }
    }
}