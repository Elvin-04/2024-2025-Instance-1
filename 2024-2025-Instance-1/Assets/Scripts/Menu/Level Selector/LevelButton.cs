using UnityEngine;
using System;
using UnityEngine.UI;

[RequireComponent(typeof (Button))]
public class LevelButton : MonoBehaviour
{
    [SerializeField] private LevelInfo _level;

    private Button _btn;

    private void Start()
    {
        _btn = GetComponent<Button>();
        _btn.interactable = _level.IsUnlocked();
    }
}