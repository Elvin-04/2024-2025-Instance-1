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
                    input.y = 0;
                else
                    input.x = 0;
            }

            EventManager.instance.onMoveStarted?.Invoke(input);
        }

        if (ctx.canceled) EventManager.instance.onMoveCanceled?.Invoke();
    }

    public void OnInteract(InputAction.CallbackContext ctx)
    {
        if (ctx.performed) EventManager.instance.onInteract.Invoke();
    }

    public void OnRetry(InputAction.CallbackContext ctx)
    {
        if (ctx.started) EventManager.instance.onReloadUIRetry?.Invoke();

        if (ctx.performed) EventManager.instance.onRetry?.Invoke();

        if (ctx.canceled) EventManager.instance.onStopHoldingReload?.Invoke();
    }

    public void OnPause(InputAction.CallbackContext ctx)
    {
        if (ctx.performed) EventManager.instance.onPause.Invoke();
    }
}