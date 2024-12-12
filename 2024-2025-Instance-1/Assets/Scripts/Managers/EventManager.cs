using Grid;
using Runes;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
///     For add an event call : EventManager.Instance.Event.AddListener(Function);
///     The function should have the shape : UnityEvent
///     <params>
///         take Function(params)
///         For call an event just add this line : EventManager.Instance.Event.Invoke(params);
///         this will call all function you have add to your event
/// </summary>
public class EventManager : MonoBehaviour
{
    public static EventManager instance { get; private set; }
    public UnityEvent<Rune> addRuneToInventory { get; private set; } = new();
    public UnityEvent<Vector2> onMoveStarted { get; private set; } = new();
    public UnityEvent onMoveCanceled { get; private set; } = new();
    public UnityEvent onInteract { get; private set; } = new();
    public UnityEvent<bool> canInteract { get; private set; } = new();
    public UnityEvent onPause { get; private set; } = new();
    public UnityEvent<Rune> updateRune { get; private set; } = new();
    public UnityEvent updateDeath { get; private set; } = new();
    public UnityEvent onDeath { get; private set; } = new();
    public UnityEvent updateClock { get; private set; } = new();
    public UnityEvent onClockUpdated { get; private set; } = new();
    public UnityEvent<Vector3, Cell> onChangeCell { get; private set; } = new(); //=> position of the cell, tile to set 
    public UnityEvent<Vector3> onResetCell { get; private set; } = new(); //=> position of the cell to change
    public UnityEvent onRetry { get; private set; } = new();
    public UnityEvent onReloadUIRetry { get; private set; } = new();
    public UnityEvent onStopHoldingReload { get; private set; } = new();
    public UnityEvent onWin { get; private set; } = new();
    public UnityEvent<Vector3> stopInteract { get; private set; } = new();
    public UnityEvent<Vector3, CellObjectBase> onRemoveObjectOnCell { get; private set; } = new();
    public UnityEvent<Vector3> onPlayerMoved { get; private set; } = new();

    // Audio
    public UnityEvent<string> onPlayMusic { get; private set; } = new();
    public UnityEvent onPlayAllMusic { get; private set; } = new();
    public UnityEvent<string> onPauseMusic { get; private set; } = new();
    public UnityEvent onPauseAllMusic { get; private set; } = new();
    public UnityEvent<string> onStopMusic { get; private set; } = new();
    public UnityEvent onStopAllMusic { get; private set; } = new();

    public UnityEvent<string> onPlaySfx { get; private set; } = new();
    public UnityEvent onPlayAllSfx { get; private set; } = new();
    public UnityEvent<string> onPauseSfx { get; private set; } = new();
    public UnityEvent onPauseAllSfx { get; private set; } = new();
    public UnityEvent<string> onStopSfx { get; private set; } = new();
    public UnityEvent onStopAllSfx { get; private set; } = new();


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            return;
        }

        Destroy(gameObject);
    }
}