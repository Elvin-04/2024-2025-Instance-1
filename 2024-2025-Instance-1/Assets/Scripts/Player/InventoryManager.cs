using UnityEngine;

namespace Player
{
    public class InventoryManager : MonoBehaviour
    {
        public Rune currentRune { get; private set; }

        private void Start()
        {
            EventManager.Instance.AddRuneToInventory.AddListener(TakeRune);
        }
    
        public void TakeRune(Rune rune)
        {
            //if (currentRune == rune || rune == null)
            //    return;
            currentRune = rune;
        }
    }
}