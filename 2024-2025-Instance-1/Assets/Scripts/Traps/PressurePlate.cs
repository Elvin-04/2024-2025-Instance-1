using System;
using System.Collections.Generic;
using Creators;
using Grid;
using UnityEngine;
using UnityEngine.Assertions;

namespace Traps
{
    public class PressurePlate : CellObjectBase, IInteractable, IWeightInteractable
    {

        public Action onPlate;
        public Action offPlate;

        [Header("Pressure Plate")]
        [SerializeField] private Cell _platePress;
        [SerializeField] private Cell _plateRelease;

        [Header("Door")]
        [SerializeField] private Cell _doorOpen;
        [SerializeField] private Cell _doorClose;

        [Header("Pillar")]
        [SerializeField] private Cell _wallCloseToDoorOpenedRight;
        [SerializeField] private Cell _wallCloseToDoorOpenedLeft;

        [SerializeField] private Cell _wallCloseToDoorClosedLeft;
        [SerializeField] private Cell _wallCloseToDoorClosedRight;

        [Header("Transform List")]
        private List<Transform> _doorTransforms = new();
        private List<PillarObject> _pillars = new();

        private void Start()
        {
            Assert.IsNotNull(_doorOpen, "door open is null in DoorButton");
            Assert.IsNotNull(_doorClose, "door close is null in DoorButton");
            Assert.IsNotNull(_wallCloseToDoorOpenedRight, "wall close to door opened is null in DoorButton Right");
            Assert.IsNotNull(_wallCloseToDoorOpenedLeft, "wall close to door opened is null in DoorButton Left");
            Assert.IsNotNull(_wallCloseToDoorClosedLeft, "wall close to door closed left is null in DoorButton");
            Assert.IsNotNull(_wallCloseToDoorClosedRight, "wall close to door closed right is null in DoorButton");
            Close();
        }

        public bool canPickUp
        {
            get => false;
            set { }
        }

        public void Interact()
        {
            Open();
            onPlate.Invoke();
        }

        public void StopInteract()
        {
            Close();
            offPlate.Invoke();
        }

        public void WeightInteract() => Interact();
        public void StopWeightInteract() => StopInteract();

        public Cell GetTileBasedOnState(bool isPress)
        {
            return isPress ? _platePress : _plateRelease;
        }
        public void SetDoorTransforms(List<Transform> transforms)
        {
            _doorTransforms = transforms;
        }
        public void SetPillars(List<PillarObject> pillars)
        {
            _pillars = pillars;
        }

        private void Open()
        {
            if (_doorTransforms == null || _pillars == null) return;

            EventManager.instance.onChangeCell?.Invoke(transform.position, _platePress);
            foreach (Transform doorTransform in _doorTransforms) OpenDoor(doorTransform);
            foreach (PillarObject pillar in _pillars) OpenPillar(pillar);
        }

        private void Close()
        {
            EventManager.instance.onChangeCell?.Invoke(transform.position, _plateRelease);
            foreach (Transform doorTransform in _doorTransforms) CloseDoor(doorTransform);
            foreach (PillarObject pillar in _pillars) ClosePillar(pillar);
        }

        private void OpenDoor(Transform doorTransform)
        {
            EventManager.instance.onChangeCell?.Invoke(doorTransform.position, _doorOpen);
        }

        private void CloseDoor(Transform doorTransform)
        {
            EventManager.instance.onChangeCell?.Invoke(doorTransform.position, _doorClose);
        }

        private void OpenPillar(PillarObject pillar)
        {
            EventManager.instance.onChangeCell?.Invoke(pillar.transform.position, 
                pillar.side == Side.Right ? _wallCloseToDoorOpenedRight : _wallCloseToDoorOpenedLeft);
        }

        private void ClosePillar(PillarObject pillar)
        {
            EventManager.instance.onChangeCell?.Invoke(pillar.transform.position,
                pillar.side == Side.Right ? _wallCloseToDoorClosedLeft : _wallCloseToDoorClosedRight) ;
        }

    }
}