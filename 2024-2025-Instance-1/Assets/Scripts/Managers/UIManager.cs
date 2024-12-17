using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

public class UIManager : MonoBehaviour
{
    [Space] [SerializeField] private GameObject _winPanel;

    [SerializeField] private GameObject _popUpInteractable;

    [Header("Can Interact")]
    [SerializeField] private TextMeshProUGUI _canInteractText;
    [SerializeField] private string _interactionText = $"Take {0} by press F";


    private void Start()
    {
        Assert.IsNotNull(_popUpInteractable, "pop up interactable is null in UIManager");
        EventManager manager = EventManager.instance;

        manager.onPause.AddListener(Pause);
        manager.canInteract.AddListener(PopUpInteract);

        manager.onWin.AddListener(OnWin);
    }

    private void PopUpInteract(bool canInteract, string objName)
    {
        if (!_popUpInteractable.activeSelf && !canInteract)
            return;

        _popUpInteractable.SetActive(canInteract);
        _canInteractText.text = string.Format(_interactionText, objName);
    }

    public void Pause()
    {
        Time.timeScale = 0f;
    }

    private void OnWin()
    {
        _winPanel.SetActive(true);
    }

    public void RestartLevelButton()
    {
        EventManager.instance.onRetry.Invoke();
    }
}