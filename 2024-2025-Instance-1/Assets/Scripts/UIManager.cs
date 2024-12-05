using System;
using TMPro;
using UnityEngine;
using System.Collections;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text _runeText;
    [SerializeField] private string _runeTextFormat = "Rune Name: {0}";

    [Space]
    [SerializeField] private GameObject _winPanel;

    [SerializeField] private GameObject _popUpInteractible;

    private void Start()
    {
        EventManager manager = EventManager.Instance;

        manager.OnPause.AddListener(Pause);
        manager.UpdateRune.AddListener(UpdateRune);
        manager.UpdateDeath.AddListener(UpdateDeath);
        manager.CanInteract.AddListener(PopUpInteract);
    }

    private void PopUpInteract(bool canInteract)
    {
        if (_popUpInteractible)
            _popUpInteractible.SetActive(canInteract);
    }

    public void Pause()
    {
        Time.timeScale = 0f;
    }

    public void UpdateRune(Rune rune)
    {
        _runeText.text = string.Format(_runeTextFormat, rune.name);
    }
    
    public void UpdateDeath()
    {
        
    }



    public void Win()
    {
        if (_winPanel.activeSelf)
            return;

        Debug.Log("W");
        
        _winPanel.SetActive(true);
        StartCoroutine(Utils.InvokeAfter(() => _winPanel.SetActive(false), 1.0f));
    }
}
