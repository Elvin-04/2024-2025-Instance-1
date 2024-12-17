using Runes;
using UnityEngine;

namespace Player
{
    public class InventoryManager : MonoBehaviour
    {
        [SerializeField] private Animator _animatorAura;
        [SerializeField] private Animator _animatorZone;
        public Rune currentRune { get; private set; }

        private void Start()
        {
            EventManager.instance.addRuneToInventory.AddListener(TakeRune);
            EventManager.instance.onRuneDropped.AddListener(DropRune);
        }

        private void DropRune()
        {
            currentRune?.DropRune();
        }

        public void TakeRune(Rune rune)
        {
            currentRune = rune;
            if (rune == null)
            {
                _animatorAura.Play("None");
                _animatorZone.Play("None");
            }
            else
            {
                rune.PlayAnimation(_animatorAura, _animatorZone);
            }
        }
    }
}