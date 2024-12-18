using Grid;
using Managers.Audio;
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
    public UnityEvent<bool, string> canInteract { get; private set; } = new();
    public UnityEvent onPause { get; private set; } = new();
    public UnityEvent<Rune> updateRune { get; private set; } = new();
    public UnityEvent onRespawn { get; private set; } = new();
    public UnityEvent<bool> onDeath { get; private set; } = new();
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
    public UnityEvent onEnableInput { get; private set; } = new();
    public UnityEvent onDisableInput { get; private set; } = new();
    public UnityEvent<int> onScoreUpdated { get; private set; } = new();
    public UnityEvent<int> OnZoneEffect { get; private set; } = new();
    public UnityEvent StopZoneEffect { get; private set; } = new();
    public UnityEvent<Vector3> onPlayerFinishedMoving { get; private set; } = new();
    public UnityEvent onRuneDropped { get; private set; } = new();
    public UnityEvent<(int, int)> onCellChanged { get; private set; } = new();
    public UnityEvent onPoisonedPlayer { get; private set; } = new();

    // Audio
    public UnityEvent<SoundsName> onPlayMusic { get; private set; } = new();
    public UnityEvent onPauseMusic { get; private set; } = new();
    public UnityEvent onStopMusic { get; private set; } = new();

    public UnityEvent<SoundsName> onPlaySfx { get; private set; } = new();
    public UnityEvent onPauseSfx { get; private set; } = new();
    public UnityEvent onStopSfx { get; private set; } = new();


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