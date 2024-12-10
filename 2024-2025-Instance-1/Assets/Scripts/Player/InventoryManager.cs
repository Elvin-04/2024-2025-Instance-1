using Runes;
using UnityEngine;

namespace Player
{
    public class InventoryManager : MonoBehaviour
    {
        public Rune currentRune { get; private set; }

        private void Start()
        {
            EventManager.instance.addRuneToInventory.AddListener(TakeRune);
        }

        public void TakeRune(Rune rune)
        {
            currentRune = rune;
        }
    }
}