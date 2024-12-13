using Runes;
using UnityEngine;

namespace Player
{
    public class InventoryManager : MonoBehaviour
    {
        public Rune currentRune { get; private set; }
        [SerializeField] private Animator _animatorAura;
        [SerializeField] private Animator _animatorZone;

        private void Start()
        {
            EventManager.instance.addRuneToInventory.AddListener(TakeRune);

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
                rune.PlayAnimation(_animatorAura,_animatorZone);

        }
    }
}