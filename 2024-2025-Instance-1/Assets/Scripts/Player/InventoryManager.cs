using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    private Rune _currentRune;

    private void Start()
    {
        EventManager.Instance.AddRuneToInventory.AddListener(TakeRune);
    }
    public void TakeRune(Rune rune)
    {
        if (_currentRune == rune || rune == null)
            return;
        _currentRune = rune;
    }
}