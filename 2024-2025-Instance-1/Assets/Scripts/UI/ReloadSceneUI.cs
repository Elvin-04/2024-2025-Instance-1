using System.Collections;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ReloadSceneUI : MonoBehaviour
{
    [SerializeField] private Image fillAmountImage;

    private float holdDuration = 1f;
    private float holdTimer = 0f;

    private bool isHolding = false;

    private void Start()
    {
        EventManager.Instance.OnReloadUIRetry.AddListener(UpdateUIReloadScene);
        EventManager.Instance.OnStopHoldingReload.AddListener(CancelReload);
    }
    private void Update()
    {
        if (isHolding)
        {
            holdTimer += Time.deltaTime;
            fillAmountImage.fillAmount = holdTimer / holdDuration;
            if (holdTimer >= holdDuration)
            {
                isHolding = false;
            }
        }
        else 
        { 
            holdTimer = 0;
            fillAmountImage.fillAmount = holdTimer / holdDuration;
        }
    }
    public void UpdateUIReloadScene( )
    {
        isHolding = true;
    }

    private void CancelReload()
    {
       isHolding = false;
    }
}
