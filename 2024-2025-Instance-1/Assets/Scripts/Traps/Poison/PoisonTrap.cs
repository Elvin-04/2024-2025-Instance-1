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
            if(creator != null)
            {
                creator.PoisonPlayer();
            }
        }

        public void StopInteract()
        {
        }

        public void WeightInteract()
        {
            if (creator != null)
            {
                creator.WeightInteract(this);
            }
        }

        public void StopWeightInteract()
        {
            if (creator != null)
            {
                creator?.StopWeightInteract(this);
            }
        }
    }
}