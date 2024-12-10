using System.Collections.Generic;
using Grid;
using NUnit.Framework;
using UnityEngine;

namespace Creators
{
    public class CreateRune : SwitchableCellCreator
    {
        [SerializeField] private GridManager _gridManager;

        protected override void Start()
        {
            Assert.IsNotNull(_gridManager, "grid manager is null in CreateRune");
            base.Start();
        }

        protected override void SetTile(Cell cell)
        {
            EventManager.Instance.OnChangeCell?.Invoke(transform.position, GetTileBasedOnState(true));
            Invoke(nameof(SetupObjectsOnCell), 0);
        }

        private void SetupObjectsOnCell()
        {
            _gridManager.GetCellObjectsByType(transform.position, out List<Rune> spawnedRunes);
            foreach (Rune rune in spawnedRunes)
            {
                rune.onDrop += OnDrop;
                rune.onTake += OnTake;
            }
        }

        private void OnTake()
        {
            EventManager.Instance.OnChangeCell?.Invoke(transform.position, GetTileBasedOnState(false));
        }

        private void OnDrop()
        {
            EventManager.Instance.OnChangeCell?.Invoke(transform.position, GetTileBasedOnState(true));
            SetupObjectsOnCell();
        }
    }
}