using System;
using Grid;
using Player;
using UnityEngine;
using UnityEngine.Assertions;

namespace DeathSystem
{
    [RequireComponent(typeof(InventoryManager))]
    public class DeathManager : MonoBehaviour
    {
        [SerializeField] private GameObject _playerCorpse;
        private GridManager _gridManager;

        //Components
        private InventoryManager _inventoryManager;
        private Transform _transform;

        //Actions
        public Action<GameObject> onPlayerDeath;

        private void Awake()
        {
            Assert.IsNotNull(_playerCorpse, "player corpse cell prefab is null in DeathManager");
            _inventoryManager = GetComponent<InventoryManager>();
            _transform = transform;
        }

        private void Start()
        {
            EventManager.Instance.OnDeath?.AddListener(Death);
        }

        //To be called when player is instantiated
        public void SetGridManager(GridManager gridManager)
        {
            _gridManager = gridManager;
        }

        private void Death()
        {
            Assert.IsNotNull(_inventoryManager);
            if (_inventoryManager.currentRune == null)
            {
                GameObject playerCorpse = Instantiate(_playerCorpse, _transform.position, Quaternion.identity);
                Corpse corpse = playerCorpse.GetComponent<Corpse>();
                _gridManager.AddObjectOnCell(_transform.position, corpse);
            }
            else
            {
                _inventoryManager.currentRune.ApplyEffect(transform.position, _gridManager);
            }

            onPlayerDeath?.Invoke(gameObject);
        }
    }
}