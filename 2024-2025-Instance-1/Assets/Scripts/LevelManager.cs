using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private static LevelManager _instance;

    [Tooltip("Maximum number of deaths allowed. 0 means unlimited deaths.")]
    [Range(0, 100)] public int maxDeath = 0;

    public int currentDeath = 0;

    private void Start()
    {
        if (_instance)
        {
            Destroy(gameObject);
            return;
        } 

        _instance = this;

        EventManager.Instance.UpdateTimer.AddListener(UpdateTimer);
    }

    public void Retry()
    {
        Debug.Log("Retry Level");
    }

    public void Win()
    {

    }

    public void Lose()
    {

    }

    public void Pause()
    {

    }

    public void UpdateTimer(float currentTime)
    {

    }

    public void OnDeath()
    {
        if (maxDeath != 0)
        {
            if (++currentDeath == maxDeath)
            {
                Debug.Log("TODO maximum number of deaths reached");
            }
        }
    }
}
