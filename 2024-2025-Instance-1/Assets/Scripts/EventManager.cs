using Grid;
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
    public UnityEvent<Rune> AddRuneToInventory { get; private set; } = new UnityEvent<Rune>();
    public UnityEvent<Vector2> OnMoveStarted { get; private set; } = new UnityEvent<Vector2>();
    public UnityEvent OnMoveCanceled { get; private set; } = new UnityEvent();
    public UnityEvent OnInteract { get; private set; } = new UnityEvent();
    public UnityEvent<bool> CanInteract { get; private set; } = new UnityEvent<bool>();
    public UnityEvent OnPause { get; private set; } = new UnityEvent();
    public UnityEvent<Rune> UpdateRune { get; private set; } = new UnityEvent<Rune>();
    public UnityEvent UpdateDeath { get; private set; } = new UnityEvent();
    public UnityEvent OnDeath { get; private set; } = new UnityEvent();
    public UnityEvent UpdateClock { get; private set; } = new UnityEvent();
    public UnityEvent OnClockUpdated { get; private set; } = new UnityEvent();
    public UnityEvent<Vector3, Cell> OnChangeCell { get; private set; } = new(); //=> position of the cell, tile to set 
    public UnityEvent<Vector3> OnResetCell { get; private set; } = new(); //=> position of the cell to change
    public UnityEvent OnRetry { get; private set; } = new UnityEvent();
    public UnityEvent OnReloadUIRetry { get; private set; } = new UnityEvent();
    public UnityEvent OnStopHoldingReload { get; private set; } = new UnityEvent();
    public UnityEvent OnWin { get; private set; } = new UnityEvent();
    public UnityEvent<Vector3> StopInteract { get; private set; } = new UnityEvent<Vector3>();
    public UnityEvent<Vector3, CellObjectBase> OnRemoveObjectOnCell { get; private set; } = new UnityEvent<Vector3, CellObjectBase>();
    public UnityEvent<string> OnTransitionScene { get; private set; } = new UnityEvent<string>();
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

