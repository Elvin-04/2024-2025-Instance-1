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
        private Transform _cloneSpawnPosition;
        [SerializeField] private Color _cloneColor;

        public void SetupCloningRune(Vector3 start, Vector3 end, PlayerManager playerManager, Transform cloneSpawnPosition)
        {
            _mirrorLineStart = start;
            _mirrorLineEnd = end;
            _playerManager = playerManager;
            _cloneSpawnPosition = cloneSpawnPosition;
        }

        public override void ApplyEffect(Vector3 position, GridManager gridManager)
        {
            Vector3 lineDir = (_mirrorLineEnd - _mirrorLineStart).normalized;
            Vector3 posToStart = position - _mirrorLineStart;
            float projectedLength = Vector3.Dot(posToStart, lineDir);
            Vector3 projectedPoint = _mirrorLineStart + lineDir * projectedLength;
            Vector3 oppositePos = projectedPoint + (projectedPoint - position);
            //
            //_spawnedPlayer = _playerManager.SpawnPlayer(oppositePos);
            _spawnedPlayer = _playerManager.SpawnPlayer(_cloneSpawnPosition.position, _cloneColor);
            _spawnedPlayer.GetComponent<DeathManager>().onPlayerDeath = DropRune;
        }

        private void DropRune(GameObject player)
        {
            DropRune();
        }
        public override void PlayAnimation(Animator animatorAura, Animator animatorZone)
        {
            animatorAura.Play(nameof(CloningRune));
            animatorZone.Play("None");
        }

        public override void DropRune()
        {
            Destroy(_spawnedPlayer);
            base.DropRune();
        }
    }
}