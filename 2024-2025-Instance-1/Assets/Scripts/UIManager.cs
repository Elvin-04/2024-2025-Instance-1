using UnityEngine;

public class UIManager : MonoBehaviour
{
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
        
    }

    public void UpdateRune(GameObject rune)
    {
        
    }
    
    public void UpdateDeath()
    {
        
    }

    public void UpdateTimer(float currentTime)
    {
        
    }
}
