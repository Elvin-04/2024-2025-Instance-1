using System;
using Grid;
using Managers.Audio;
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

        public void Interact()
        {
            onTake?.Invoke();
            EventManager.instance.addRuneToInventory.Invoke(this);
            EventManager.instance.onPlaySfx?.Invoke(SoundsName.Collectible, null);
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