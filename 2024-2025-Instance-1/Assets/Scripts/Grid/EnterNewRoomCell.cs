using DG.Tweening;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Grid
{
    public class EnterNewRoomCell : CellObjectBase, IInteractable
    {
        [SerializeField] private Vector2Int _previousCamPos;
        [SerializeField] private Vector2Int _nextCamPos;

        private bool _posIsNext = true;

        public bool CanPickUp {get; set;} = false;        

        public void Interact()
        {
            Vector2Int nextCamPos = _posIsNext ? _nextCamPos : _previousCamPos;
            _posIsNext = !_posIsNext;

            Camera.main.transform.DOMove(new Vector3(nextCamPos.x, nextCamPos.y, Camera.main.transform.position.z), 0.5f).SetEase(Ease.OutCubic);
        }
    }
}