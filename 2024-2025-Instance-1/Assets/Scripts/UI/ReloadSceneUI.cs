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

            _fillAmountImage.type = Image.Type.Filled;

            Reset();
        }

        private void Update()
        {
            if (_isHolding)
            {
                _holdTimer += Time.deltaTime;
                _fillAmountImage.fillAmount = _holdTimer / _holdDuration;

                if (_holdTimer >= _holdDuration) 
                    _isHolding = false;
            }
            else
            {
                Reset();
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

        private void Reset()
        {
            _holdTimer = 0f;
            _fillAmountImage.fillAmount = 0f;
        }
    }
}