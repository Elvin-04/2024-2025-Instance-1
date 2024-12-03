using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text _lifeText;
    [SerializeField] private string _lifeTextFormat = "{0}";


    [SerializeField] private TMP_Text _runeText;
    [SerializeField] private string _runeTextFormat = "Rune Name: {0}";

    private void Start()
    {
        EventManager manager = EventManager.Instance;

        manager.OnPause.AddListener(Pause);
        manager.UpdateTimer.AddListener(UpdateTimer);
        manager.UpdateLife.AddListener(UpdateLife);
        manager.UpdateRune.AddListener(UpdateRune);
        manager.UpdateDeath.AddListener(UpdateDeath);
    }

    public void Pause()
    {
        
    }

    public void UpdateLife(int currentLife)
    {
        _lifeText.text = string.Format(_lifeTextFormat, currentLife.ToString());
    }

    public void UpdateRune(GameObject rune)
    {
        _runeText.text = string.Format(_runeTextFormat, rune.name);
    }
    
    public void UpdateDeath()
    {
        
    }

    public void UpdateTimer(float currentTime)
    {
        
    }
}
