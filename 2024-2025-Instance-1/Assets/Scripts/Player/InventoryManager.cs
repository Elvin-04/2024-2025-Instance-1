using UnityEngine;

namespace Player
{
    public class InventoryManager : MonoBehaviour
    {
        public Rune currentRune { get; private set; }
        public Rune explosiveRune;                  //To remove                                 


        private void Start()
        {
            EventManager.Instance.AddRuneToInventory.AddListener(TakeRune);
            currentRune = explosiveRune;             //To remove
        }
    
        public void TakeRune(Rune rune)
        {
            currentRune = rune;
        }
    }
}