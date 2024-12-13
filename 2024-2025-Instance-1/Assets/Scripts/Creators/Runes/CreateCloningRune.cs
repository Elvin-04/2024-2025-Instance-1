using Player;
using Runes;
using UnityEngine;
using UnityEngine.Assertions;

namespace Creators.Runes
{
    public class CreateCloningRune : CreateRune
    {
        [SerializeField] private Transform[] _lineEnds = new Transform[2];
        [SerializeField] private PlayerManager _playerManager;
        private CloningRune spawnedCloningRune => _spawnedRune as CloningRune;

        protected override void Start()
        {
            Assert.IsFalse(_lineEnds.Length != 2, "Cloning rune must have 2 line ends");
            foreach (Transform lineEnd in _lineEnds)
            {
                Assert.IsNotNull(lineEnd, "Line end is null");
            }

            Assert.IsFalse(_spawnedRune is CloningRune, "Rune must be cloning rune");
            Assert.IsNotNull(_playerManager, "Player prefab is null");
            base.Start();
        }

        protected override void OnDrawGizmos()
        {
            base.OnDrawGizmos();
            Assert.IsFalse(_lineEnds.Length != 2, "Cloning rune must have 2 line ends");
            foreach (Transform lineEnd in _lineEnds)
            {
                Assert.IsNotNull(lineEnd, "Line end is null");
            }

            Gizmos.color = Color.white;
            Gizmos.DrawLine(_lineEnds[0].position, _lineEnds[1].position);
        }

        protected override void SetupObjectsOnCell()
        {
            base.SetupObjectsOnCell();
            spawnedCloningRune.SetupCloningRune(_lineEnds[0].position, _lineEnds[1].position, _playerManager);
        }
    }
}