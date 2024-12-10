using System.Collections.Generic;
using Grid;
using UnityEngine;
using UnityEngine.Assertions;

namespace Traps
{
    public class DoorButton : CellObjectBase, IInteractable
    {
        [SerializeField] private Cell _doorOpen;
        [SerializeField] private Cell _doorClose;

        [SerializeField] private Cell _wallCloseToDoorOpened;

        [SerializeField] private Cell _wallCloseToDoorClosedLeft;
        [SerializeField] private Cell _wallCloseToDoorClosedRight;
        private List<Transform> _doorTransforms = new();

        private void Start()
        {
            Assert.IsNotNull(_doorOpen, "door open is null in DoorButton");
            Assert.IsNotNull(_doorClose, "door close is null in DoorButton");
            Assert.IsNotNull(_wallCloseToDoorOpened, "wall close to door opened is null in DoorButton");
            Assert.IsNotNull(_wallCloseToDoorClosedLeft, "wall close to door closed left is null in DoorButton");
            Assert.IsNotNull(_wallCloseToDoorClosedRight, "wall close to door closed right is null in DoorButton");
        }

        public bool canPickUp
        {
            get => false;
            set { }
        }

        public void Interact()
        {
            OpenDoors();
        }

        public void StopInteract()
        {
            CloseDoors();
        }

        public void SetDoorTransforms(List<Transform> transforms)
        {
            _doorTransforms = transforms;
        }

        private void OpenDoors()
        {
            if (_doorTransforms == null) return;

            foreach (Transform doorTransform in _doorTransforms) Open(doorTransform);
        }

        private void CloseDoors()
        {
            foreach (Transform doorTransform in _doorTransforms) Close(doorTransform);
        }

        private void Open(Transform doorTransform)
        {
            EventManager.instance.onChangeCell?.Invoke(doorTransform.position, _doorOpen);
            EventManager.instance.onChangeCell?.Invoke(doorTransform.position + Vector3.right, _wallCloseToDoorOpened);
            EventManager.instance.onChangeCell?.Invoke(doorTransform.position + Vector3.left, _wallCloseToDoorOpened);
        }

        private void Close(Transform doorTransform)
        {
            EventManager.instance.onChangeCell?.Invoke(doorTransform.position, _doorClose);
            EventManager.instance.onChangeCell?.Invoke(doorTransform.position + Vector3.right,
                _wallCloseToDoorClosedLeft);
            EventManager.instance.onChangeCell?.Invoke(doorTransform.position + Vector3.left,
                _wallCloseToDoorClosedRight);
        }
    }
}