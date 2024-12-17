using Grid;
using Managers.Audio;
using System;
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

        public virtual string showName => "Rune";

        public void Interact()
        {
            onTake?.Invoke();
            EventManager.instance.canInteract.Invoke(false, "");
            EventManager.instance.addRuneToInventory.Invoke(this);
            EventManager.instance.onPlaySfx?.Invoke(SoundsName.Collectible, null);
        }

        public void StopInteract()
        {
        }

        public virtual void PlayAnimation(Animator animatorAura, Animator animatorZone)
        {
            animatorAura.Play("None");
            animatorZone.Play("None");
        }

        public abstract void ApplyEffect(Vector3 position, GridManager gridManager);

        public virtual void DropRune()
        {
            onDrop?.Invoke();
        }
    }
}