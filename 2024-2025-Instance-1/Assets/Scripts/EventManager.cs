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
    private static EventManager _instance;
    public static EventManager Instance {  get { return _instance; } }

    public UnityEvent<GameObject> AddRuneToInventory; // TODO Rune
    public UnityEvent<Vector2> OnMoveStarted;
    public UnityEvent OnMoveCanceled;
    public UnityEvent OnInteract;
    public UnityEvent OnPause;
    public UnityEvent<float> UpdateTimer;
    public UnityEvent<int> UpdateLife;
    public UnityEvent<GameObject> UpdateRune;
    public UnityEvent UpdateDeath;
    public UnityEvent OnDeath;
    private void Start()
    {
        if (_instance == null) 
        {
            _instance = this;
            return;
        }
        Destroy(this);
    }
}
