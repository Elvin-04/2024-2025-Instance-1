using System.Collections;
using DeathSystem;
using Grid;
using Managers;
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

        [SerializeField] private float _waitTimeBeforeRespawn = 1f;
        private bool _calledUpdateClockThisFrame;

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
            SpawnPlayer(_levelManager.spawnPoint.position);
            EventManager.instance.onPlayerMoved?.AddListener(OnPlayerMoved);
        }

        private void OnPlayerMoved(Vector3 pos)
        {
            if (_calledUpdateClockThisFrame) return;
            _calledUpdateClockThisFrame = true;
            EventManager.instance.updateClock?.Invoke();
            StartCoroutine(ResetUpdateClockFlagAtEndOfFrame());
        }

        private IEnumerator ResetUpdateClockFlagAtEndOfFrame()
        {
            yield return new WaitForEndOfFrame();
            _calledUpdateClockThisFrame = false;
        }

        public GameObject SpawnPlayer(Vector3 pos)
        {
            GameObject player = Instantiate(_playerPrefab, GetCellPos(pos),
                Quaternion.identity);
            DeathManager playerDeathManager = player.GetComponent<DeathManager>();
            PlayerController playerController = player.GetComponent<PlayerController>();
            playerDeathManager.SetGridManager(_gridManager);
            playerDeathManager.onPlayerDeath += OnDeath;
            playerController.SetGridManager(_gridManager);
            return player;
        }

        private Vector3 GetCellPos(Vector3 pos)
        {
            Vector3 position = _gridManager.GetCellPos(_gridManager.GetCellIndex(pos));
            return position;
        }

        private void OnDeath(GameObject player)
        {
            if (player != null) StartCoroutine(nameof(Respawn), player);
        }

        private IEnumerator Respawn(GameObject player)
        {
            if (player == null) yield break;
            yield return new WaitForEndOfFrame();
            player.SetActive(false);
            yield return new WaitForSeconds(_waitTimeBeforeRespawn);
            player.transform.position = GetCellPos(_levelManager.spawnPoint.position);
            player.SetActive(true);
            EventManager.instance.onRespawn?.Invoke();
            EventManager.instance.onPlayerMoved?.Invoke(player.transform.position);
        }
    }
}