using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class LevelManager : MonoBehaviour
    {
        private static LevelManager _instance;

        [field: SerializeField] public Transform spawnPoint { get; private set; }
        [field: SerializeField] public ScoreCounter maxDeaths { get; private set; }
        [field: SerializeField] public ScoreCounter maxSteps { get; private set; }

        private void Awake()
        {
            Assert.IsNotNull(spawnPoint, "spawnPoint is null in LevelManager");
        }

        private void Start()
        {
            if (_instance)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
            EventManager.instance.onRetry.AddListener(ReloadScene);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.grey;
            Gizmos.DrawCube(spawnPoint.position, Vector3.one * 0.5f);
        }

        public void ReloadScene()
        {
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadSceneAsync(scene.name);
        }
    }
}