using UnityEngine;
using UnityEngine.UI;

public class ReloadSceneUI : MonoBehaviour
{
    [SerializeField] private Image _fillAmountImage;

    private float _holdDuration = 1f;
    private float _holdTimer = 0f;

    private bool _isHolding = false;

    private void Start()
    {
        EventManager.Instance.OnReloadUIRetry.AddListener(UpdateUIReloadScene);
        EventManager.Instance.OnStopHoldingReload.AddListener(CancelReload);
    }
    private void Update()
    {
        if (_isHolding)
        {
            _holdTimer += Time.deltaTime;
            _fillAmountImage.fillAmount = _holdTimer / _holdDuration;
            if (_holdTimer >= _holdDuration)
            {
                _isHolding = false;
            }
        }
        else 
        { 
            _holdTimer = 0;
            _fillAmountImage.fillAmount = _holdTimer / _holdDuration;
        }
    }
    private void UpdateUIReloadScene( )
    {
        _isHolding = true;
    }

    private void CancelReload()
    {
       _isHolding = false;
    }
}
