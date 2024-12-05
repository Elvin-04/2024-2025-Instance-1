using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// For add an event call : EventManager.Instance.Event.AddListener(Function);
/// The function should have the shape : UnityEvent<params> take Function(params)
/// For call an event just add this line : EventManager.Instance.Event.Invoke(params);
/// this will call all function you have add to your event
/// </summary>

public class EventManager : MonoBehaviour
{
    public static EventManager Instance { get; private set; }

    public UnityEvent<GameObject> AddRuneToInventory { get; private set; } // TODO Rune
    public UnityEvent<Vector2> OnMoveStarted  { get; private set; }
    public UnityEvent OnMoveCanceled { get; private set; }
    public UnityEvent OnInteract { get; private set; }
    public UnityEvent OnPause { get; private set; }

    public UnityEvent<int> UpdateLife { get; private set; }
    public UnityEvent<GameObject> UpdateRune { get; private set; }
    public UnityEvent UpdateDeath { get; private set; }
    public UnityEvent OnDeath { get; private set; }
    public UnityEvent UpdateClock { get; private set; }
    public UnityEvent OnClockUpdated { get; private set; }

    private void Awake()
    {
        if (Instance == null) 
        {
            Instance = this;
            return;
        }
        Destroy(this);
    }
}
