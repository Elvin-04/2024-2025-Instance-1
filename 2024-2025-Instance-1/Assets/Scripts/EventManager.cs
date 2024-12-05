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
    public static EventManager Instance { get; private set; } = new();
    public UnityEvent<Rune> AddRuneToInventory { get; private set; }  = new();
    public UnityEvent<Vector2> OnMoveStarted  { get; private set; }  = new();
    public UnityEvent OnMoveCanceled { get; private set; }  = new();
    public UnityEvent OnInteract { get; private set; }  = new();
    public UnityEvent OnPause { get; private set; }  = new();

    public UnityEvent<int> UpdateLife { get; private set; }  = new();
    public UnityEvent<Rune> UpdateRune { get; private set; }  = new();
    public UnityEvent UpdateDeath { get; private set; }  = new();
    public UnityEvent OnDeath { get; private set; }  = new();
    public UnityEvent UpdateClock { get; private set; }  = new();
    public UnityEvent OnClockUpdated { get; private set; }  = new();


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
