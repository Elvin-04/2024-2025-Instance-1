using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace UI
{
    public class ReloadSceneUI : MonoBehaviour
    {
        private const float _holdDuration = 1f;
        [SerializeField] private Image _fillAmountImage;
        private float _holdTimer;

        private bool _isHolding;

        private void Start()
        {
            Assert.IsNotNull(_fillAmountImage, "fill amount image is null in ReloadSceneUI");
            EventManager.instance.onReloadUIRetry.AddListener(UpdateUIReloadScene);
            EventManager.instance.onStopHoldingReload.AddListener(CancelReload);
        }

        private void Update()
        {
            if (_isHolding)
            {
                _holdTimer += Time.deltaTime;
                _fillAmountImage.fillAmount = _holdTimer / _holdDuration;
                if (_holdTimer >= _holdDuration) _isHolding = false;
            }
            else
            {
                _holdTimer = 0;
                _fillAmountImage.fillAmount = _holdTimer / _holdDuration;
            }
        }

        private void UpdateUIReloadScene()
        {
            _isHolding = true;
        }

        private void CancelReload()
        {
            _isHolding = false;
        }
    }
}