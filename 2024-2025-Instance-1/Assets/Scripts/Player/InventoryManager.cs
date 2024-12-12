using Runes;
using UnityEngine;

namespace Player
{
    public class InventoryManager : MonoBehaviour
    {
        public Rune currentRune { get; private set; }
        [SerializeField] private Animator animator;

        private void Start()
        {
            EventManager.instance.addRuneToInventory.AddListener(TakeRune);

        }

        public void TakeRune(Rune rune)
        {
            currentRune = rune;
            PlayAuraAnimation(currentRune);
        }

        private void PlayAuraAnimation(Rune rune)
        {
            if (rune == null)
                animator.Play("None");
            if(rune is ExplosionRune)
                animator.Play(nameof(ExplosionRune));

        }
    }
}