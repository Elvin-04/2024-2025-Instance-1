using DG.Tweening;
using Player;
using UnityEngine;

namespace Grid
{
    public class EnterNewRoomCell : CellObjectBase, IInteractable
    {
        public Vector2Int nextCamPos;
        public bool CanPickUp {get; set;} = false;

        private bool _canMove = true;

        public void Interact(PlayerController controller)
        {
            if (!_canMove)
                return;
            
            Vector3 newPos = new Vector3(nextCamPos.x, nextCamPos.y, Camera.main.transform.position.z);

            if (newPos == Camera.main.transform.position)
                return;

            _canMove = false;

            controller.BanDirection(controller.currentDirection.GetOpposite());
            Camera.main.transform.DOMove(newPos, 0.5f).SetEase(Ease.OutCubic).OnComplete(() => _canMove = true);
        }

        public override bool IsEqual(CellObjectBase other)
        {            
            if (other is not EnterNewRoomCell)
                return false;
            
            EnterNewRoomCell otherAsCorrectType = other as EnterNewRoomCell;
            return nextCamPos == otherAsCorrectType.nextCamPos;
        }

        public void StopInteract()
        {
        }
    }
}