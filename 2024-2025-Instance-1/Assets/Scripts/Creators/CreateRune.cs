using Grid;
using Runes;
using UnityEngine;
using UnityEngine.Assertions;

namespace Creators
{
    public class CreateRune : SwitchableCellCreator
    {
        [SerializeField] private GridManager _gridManager;
        [SerializeField] private Rune _runeToSpawn;
        private Rune _spawnedRune;

        protected override void Start()
        {
            Assert.IsNotNull(_gridManager, "grid manager is null in CreateRune");
            Assert.IsNotNull(_runeToSpawn, "rune to spawn is null in CreateRune");
            base.Start();
        }

        protected override void SetTile(Cell cell)
        {
            EventManager.instance.onChangeCell?.Invoke(transform.position, GetTileBasedOnState(true));
            _spawnedRune = Instantiate(_runeToSpawn, transform.position, Quaternion.identity);
            Invoke(nameof(SetupObjectsOnCell), 0);
        }

        private void SetupObjectsOnCell()
        {
            _gridManager.AddObjectOnCell(transform.position, _spawnedRune);
            _spawnedRune.onDrop += OnDrop;
            _spawnedRune.onTake += OnTake;
        }

        private void OnTake()
        {
            EventManager.instance.onChangeCell?.Invoke(transform.position, GetTileBasedOnState(false));
            _gridManager.RemoveObjectOnCell(transform.position, _spawnedRune);
        }

        private void OnDrop()
        {
            EventManager.instance.onChangeCell?.Invoke(transform.position, GetTileBasedOnState(true));
            SetupObjectsOnCell();
        }
    }
}