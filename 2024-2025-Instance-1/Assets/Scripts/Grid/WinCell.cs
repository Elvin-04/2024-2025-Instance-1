using Managers.Audio;

namespace Grid
{
    public class WinCell : CellObjectBase, IInteractable
    {
        public bool canPickUp { get; set; } = false;
        
        private SoundsName _jingleWin = SoundsName.JingleWin;
        public void Interact()
        {
            EventManager.instance.onWin.Invoke();
            EventManager.instance.onDisableInput.Invoke();
            EventManager.instance.onPlaySfx?.Invoke(_jingleWin);
        }

        public void StopInteract()
        {
        }
    }
}