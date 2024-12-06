using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public void OnMove(InputAction.CallbackContext ctx)
    {
        Vector2 input = ctx.ReadValue<Vector2>();
        if (ctx.performed)
        {
            if (input.x != 0 && input.y != 0)
            {
                if (Mathf.Abs(input.x) > Mathf.Abs(input.y))
                {
                    input.y = 0;
                }
                else
                {
                    input.x = 0;
                }
            }
            EventManager.Instance.OnMoveStarted?.Invoke(input);
        }

        if (ctx.canceled)
        {
            EventManager.Instance.OnMoveCanceled?.Invoke();
        }
    }

    public void OnInteract(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            EventManager.Instance.OnInteract.Invoke();
        }

        if (ctx.canceled)
        {
            //_onInteractCanceled?.Invoke();
        }
    }

    public void OnRetry(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            EventManager.Instance.OnReloadUIRetry?.Invoke();
        }

        if (ctx.performed)
        {
            EventManager.Instance.OnRetry?.Invoke();
        }

        if (ctx.canceled)
        {
            EventManager.Instance.OnStopHoldingReload?.Invoke();
        }
    }

    public void OnPause(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            EventManager.Instance.OnPause.Invoke();
        }

        if (ctx.canceled)
        {
            //_onPauseCanceled?.Invoke();
        }
    }
}