using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    private Rune currentRune;

    private void Start()
    {
        EventManager.Instance.AddRuneToInventory.AddListener(TakeRune);
    }
    public void TakeRune(Rune rune)
    {
        if (currentRune == rune || rune == null)
            return;
        currentRune = rune;
    }
}