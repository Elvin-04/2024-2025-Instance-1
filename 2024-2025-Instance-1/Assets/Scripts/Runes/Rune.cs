using System;
using Grid;
using UnityEngine;

namespace Runes
{
    public abstract class Rune : CellObjectBase, IInteractableCallable, ICollisionObject
    {
        protected bool _canPickUp = true;

        public Action onDrop;
        public Action onTake;

        public bool canPickUp
        {
            get => _canPickUp;
            set => _canPickUp = value;
        }
        public virtual string showName { get => "Rune";}

        public void Interact()
        {
            onTake?.Invoke();
            EventManager.instance.canInteract.Invoke(false, "");
            EventManager.instance.addRuneToInventory.Invoke(this);
        }

        public virtual void PlayAnimation(Animator animatorAura, Animator animatorZone)
        {
            animatorAura.Play("None");
            animatorZone.Play("None");
        }

        public void StopInteract()
        {
        }

        public abstract void ApplyEffect(Vector3 position, GridManager gridManager);

        public void DropRune()
        {
            onDrop?.Invoke();
        }
    }
}