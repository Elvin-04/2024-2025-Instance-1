using System;
using Grid;
using Player;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Tilemaps;

namespace DeathSystem
{
    [RequireComponent(typeof(InventoryManager))]
    public class DeathManager : MonoBehaviour
    {
        [SerializeField] private TileBase _playerCorpseCell;
        
        //Components
        private InventoryManager _inventoryManager;
        private GridManager _gridManager;
        private Transform _transform;

        //Actions
        public Action<GameObject> onPlayerDeath;

        private void Awake()
        {
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
                _gridManager.SetTile(_gridManager.WorldToCell(_transform.position), _playerCorpseCell);
            }
            else
            {
                _inventoryManager.currentRune.ApplyEffect();
            }
            onPlayerDeath?.Invoke(gameObject);
        }
        
    }
}
