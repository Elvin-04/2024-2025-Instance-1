using DeathSystem;
using Grid;
using UnityEngine;
using UnityEngine.Assertions;

namespace Player
{
    [RequireComponent(typeof(LevelManager))]
    [RequireComponent(typeof(GridManager))]
    public class PlayerManager : MonoBehaviour
    {
        //Prefabs
        [Header("Prefabs")] [SerializeField] private GameObject _playerPrefab;

        //Properties
        private GameObject _currentPlayer;
        private GridManager _gridManager;

        //Components
        private LevelManager _levelManager;

        private void Awake()
        {
            Assert.IsNotNull(_playerPrefab, "player prefab is null in PlayerManager");

            _levelManager = GetComponent<LevelManager>();
            _gridManager = GetComponent<GridManager>();
        }

        private void Start()
        {
            SpawnPlayer();
        }

        private void SpawnPlayer()
        {
            Assert.IsNotNull(_levelManager);
            Assert.IsNotNull(_levelManager.spawnPoint);
            Assert.IsNotNull(_playerPrefab);
            GameObject player = Instantiate(_playerPrefab, GetCellPos(_levelManager.spawnPoint.position),
                Quaternion.identity);
            DeathManager playerDeathManager = player.GetComponent<DeathManager>();
            PlayerController playerController = player.GetComponent<PlayerController>();
            playerDeathManager.SetGridManager(_gridManager);
            playerDeathManager.onPlayerDeath += OnDeath;
            playerController.SetGridManager(_gridManager);
        }

        private Vector3 GetCellPos(Vector3 pos)
        {
            Vector3 position = _gridManager.GetCellPos(_gridManager.GetCellIndex(pos));
            return position;
        }

        private void OnDeath(GameObject player)
        {
            player.transform.position = GetCellPos(_levelManager.spawnPoint.position);
            EventManager.Instance.OnPlayerMoved?.Invoke(player.transform.position);
        }
    }
}