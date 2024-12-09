using DG.Tweening;
using Player;
using UnityEngine;

namespace Grid
{
    public class EnterNewRoomCell : CellObjectBase, IInteractable
    {
        public Vector2Int previousCamPos;
        public Vector2Int nextCamPos;

        private bool _posIsNext = true;

        public bool CanPickUp {get; set;} = false;        

        public void Interact(PlayerController controller)
        {
            Vector2Int newPos = _posIsNext ? nextCamPos : previousCamPos;
            _posIsNext = !_posIsNext;

            controller.BanDirection(controller.currentDirection.GetOpposite());
            Camera.main.transform.DOMove(new Vector3(newPos.x, newPos.y, Camera.main.transform.position.z), 0.5f).SetEase(Ease.OutCubic);
        }

        public override bool IsEqual(CellObjectBase other)
        {            
            if (other is not EnterNewRoomCell)
                return false;
            
            EnterNewRoomCell otherAsCorrectType = other as EnterNewRoomCell;
            return previousCamPos == otherAsCorrectType.previousCamPos && nextCamPos == otherAsCorrectType.nextCamPos;
        }

        public void StopInteract()
        {
        }
    }
}