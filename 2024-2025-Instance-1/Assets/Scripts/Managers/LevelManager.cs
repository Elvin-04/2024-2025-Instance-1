using UnityEngine;
using Grid;
using System.Collections.Generic;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class LevelManager : MonoBehaviour
    {
        private static LevelManager _instance;

        [SerializeField] private GridManager _gridManager;
        [SerializeField] private Cell _spawnPointCell;
        [SerializeField] private Cell _spawnPointActivatedCell;

        [field: SerializeField] public ScoreCounter maxDeaths { get; private set; }
        [field: SerializeField] public ScoreCounter maxSteps { get; private set; }

        [Tooltip("Order matters! Once a spawn point is reached, any previous spawn points will be disabled.")]
        [SerializeField] private List<Transform> spawnPoints = new List<Transform>();
        private List<(Vector2Int, Vector3)> _spawnPoints = new List<(Vector2Int, Vector3)>();

        private int _spawnPointIndex = 0;

        public Vector3 spawnPoint
        { // ugly
            get => _spawnPoints.Count == 0 ? spawnPoints[0].position : _spawnPoints[_spawnPointIndex].Item2;
        }

        private void Awake()
        {
            if (spawnPoints.Count == 0)
                throw new System.Exception("no spawn points");
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
            EventManager.instance.onPlayerFinishedMoving.AddListener(OnPlayerMoved);

            foreach (Transform spawnPoint in spawnPoints)
            {
                Vector3 spawnPos = _gridManager.GetCellPos(spawnPoint.position);
                _spawnPoints.Add((_gridManager.GetCellIndex(spawnPos), spawnPos));

                _gridManager.ChangeCell(_spawnPoints[^1].Item1, _spawnPointCell);
            }

            _gridManager.ChangeCell(_spawnPoints[0].Item1, _spawnPointActivatedCell);
        }

        public void OnPlayerMoved(Vector3 position)
        {
            if (_spawnPointIndex == spawnPoints.Count)
                return;

            Vector2Int cellIndex = _gridManager.GetCellIndex(_gridManager.GetCellPos(position));

            for (int i = _spawnPointIndex + 1; i < _spawnPoints.Count; i++)
            {
                if (_spawnPoints[i].Item1 == cellIndex)
                {
                    _gridManager.ChangeCell(_spawnPoints[i - 1].Item1, _spawnPointCell);
                    _gridManager.ChangeCell(cellIndex, _spawnPointActivatedCell);

                    _spawnPointIndex = i;
                    break;
                }
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.grey;

            foreach (Transform spawnPoint in spawnPoints)
                Gizmos.DrawCube(spawnPoint.position, Vector3.one * 0.5f);
        }

        public void ReloadScene()
        {
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadSceneAsync(scene.name);
        }
    }
}