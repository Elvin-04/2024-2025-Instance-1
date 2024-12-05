using Grid;
using System.Collections.Generic;
using UnityEngine;

public class Spike : CellObjectBase, IInteractable
{
    [SerializeField] private Sprite _spriteActive;
    [SerializeField] private Sprite _spriteInactive;
    private SpriteRenderer _spriteRenderer;

    private bool _isActive = false;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        SetSprite(_isActive);
    }
    public void UpdateSpike()
    {
        _isActive = !_isActive;
        SetSprite(_isActive);
    }

    private Sprite GetSprite(bool isActive)
    {
        return isActive ? _spriteActive : _spriteInactive;
    }

    private void SetSprite(bool isActive)
    {
        _spriteRenderer.sprite = GetSprite(_isActive);
    }

    public void Interact()
    {
        Debug.Log("Spike :: " + _isActive);
        if (!_isActive)
            return;
        Debug.Log("Kill Player");
        
    }

    public bool IsActive => _isActive;

    public bool CanPickUp { get => false; set{} }
}
