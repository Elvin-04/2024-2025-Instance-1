using UnityEngine;
using Grid;
using Creators;

namespace Traps
{

public class PoisonTrap : CellObjectBase, IInteractable, IWeightInteractable
{
    [SerializeField] private int _maxTickClock;
    private int _currentTickClock;
    private bool _playerPoisoned = false;

    [HideInInspector] public PoisonTrapCreator creator;

    public bool canPickUp { get => false; set { } }

    private void Start()
    {
        EventManager.instance.onDeath?.AddListener(OnDeath); 
        EventManager.instance.onClockUpdated?.AddListener(OnClockUpdate);
    }
    public void OnClockUpdate()
    {
        if (!_playerPoisoned)
            return;

        // yes, i++
        //  not ++i
        if (_currentTickClock++ >= _maxTickClock)
        {
            EventManager.instance.onDeath?.Invoke();
        }
    }

    public void Interact()
    {
        _playerPoisoned = true;
    }

    public void StopInteract() {}

    public void WeightInteract()
    {
        creator.WeightInteract(this);
    }

    public void StopWeightInteract()
    {
        creator.StopWeightInteract(this);
    }

    private void OnDeath()
    {
        _currentTickClock = 0;
        _playerPoisoned = false;
    }
}

}