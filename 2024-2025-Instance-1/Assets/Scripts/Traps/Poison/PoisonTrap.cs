using Creators;
using Grid;
using UnityEngine;

namespace Traps
{
    public class PoisonTrap : CellObjectBase, IInteractable, IWeightInteractable
    {
        [HideInInspector] public PoisonTrapCreator creator;

        public bool canPickUp
        {
            get => false;
            set { }
        }

        public void Interact()
        {
            creator.PoisonPlayer();
        }

        public void StopInteract()
        {
        }

        public void WeightInteract()
        {
            creator.WeightInteract(this);
        }

        public void StopWeightInteract()
        {
            creator.StopWeightInteract(this);
        }
    }
}