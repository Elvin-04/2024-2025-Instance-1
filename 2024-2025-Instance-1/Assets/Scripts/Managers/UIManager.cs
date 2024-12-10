using UnityEngine;
using UnityEngine.Assertions;

public class UIManager : MonoBehaviour
{
    [Space] [SerializeField] private GameObject _winPanel;

    [SerializeField] private GameObject _popUpInteractable;

    private void Start()
    {
        Assert.IsNotNull(_popUpInteractable, "pop up interactable is null in UIManager");
        EventManager manager = EventManager.instance;

        manager.onPause.AddListener(Pause);
        manager.canInteract.AddListener(PopUpInteract);

        manager.onWin.AddListener(OnWin);
    }

    private void PopUpInteract(bool canInteract)
    {
        if (_popUpInteractable)
            _popUpInteractable.SetActive(canInteract);
    }

    public void Pause()
    {
        Time.timeScale = 0f;
    }

    private void OnWin()
    {
        _winPanel.SetActive(true);
    }
}