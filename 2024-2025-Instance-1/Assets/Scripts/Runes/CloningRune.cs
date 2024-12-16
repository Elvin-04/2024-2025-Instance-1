using DeathSystem;
using Grid;
using Player;
using UnityEngine;

namespace Runes
{
    public class CloningRune : Rune
    {
        //Debug :
        private Vector3 _mirrorLineEnd;
        private Vector3 _mirrorLineStart;
        private PlayerManager _playerManager;
        private GameObject _spawnedPlayer;

        public void SetupCloningRune(Vector3 start, Vector3 end, PlayerManager playerManager)
        {
            _mirrorLineStart = start;
            _mirrorLineEnd = end;
            _playerManager = playerManager;
        }

        public override void ApplyEffect(Vector3 position, GridManager gridManager)
        {
            Vector3 lineDir = (_mirrorLineEnd - _mirrorLineStart).normalized;
            Vector3 posToStart = position - _mirrorLineStart;
            float projectedLength = Vector3.Dot(posToStart, lineDir);
            Vector3 projectedPoint = _mirrorLineStart + lineDir * projectedLength;
            Vector3 oppositePos = projectedPoint + (projectedPoint - position);
            _spawnedPlayer = _playerManager.SpawnPlayer(oppositePos);
            _spawnedPlayer.GetComponent<DeathManager>().onPlayerDeath += DropRune;
        }

        private void DropRune(GameObject player)
        {
            DropRune();
        }

        public override void DropRune()
        {
            Destroy(_spawnedPlayer);
            base.DropRune();
        }
    }
}