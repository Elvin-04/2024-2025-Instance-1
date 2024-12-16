using UnityEngine;
using Grid;
using Creators;

namespace Traps
{

public class PoisonTrap : CellObjectBase, IInteractable, IWeightInteractable
{

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
            EventManager.instance.onDeath?.Invoke(true);
        }
    }
    public void Interact()
    {
        creator.PoisonPlayer();
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

    private void OnDeath(bool deathEffect)
    {
        _currentTickClock = 0;
        _playerPoisoned = false;
    }
}

}