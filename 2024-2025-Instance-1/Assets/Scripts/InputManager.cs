using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
        //Actions
        //Move
        private Action<Vector3> _onMovePerformed;
        private Action<Vector3> _onMoveCanceled;
        
        //Interactions
        private Action _onInteractPerformed;
        private Action _onInteractCanceled;
        
        //Retry
        private Action _onRetryPerformed;
        private Action _onRetryCanceled;
        
        //Pause
        private Action _onPausePerformed;
        private Action _onPauseCanceled;

        public void OnMove(InputAction.CallbackContext ctx)
        {
            Vector2 input = ctx.ReadValue<Vector2>();
            if (ctx.performed)
            {
                _onMovePerformed?.Invoke(input);
            }
    
            if (ctx.canceled)
            {
                _onMoveCanceled?.Invoke(Vector3.zero);
            }
        }
        
        public void OnInteract(InputAction.CallbackContext ctx)
        {
            if (ctx.performed)
            {
                _onInteractPerformed?.Invoke();
            }

            if (ctx.canceled)
            {
                _onInteractCanceled?.Invoke();
            }
        }

        public void OnRetry(InputAction.CallbackContext ctx)
        {
            if (ctx.performed)
            {
                _onRetryPerformed?.Invoke();
            }

            if (ctx.canceled)
            {
                _onRetryCanceled?.Invoke();
            }
        }

        public void OnPause(InputAction.CallbackContext ctx)
        {
            if (ctx.performed)
            {
                _onPausePerformed?.Invoke();
            }

            if (ctx.canceled)
            {
                _onPauseCanceled?.Invoke();
            }
        }
}