using DG.Tweening;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private Camera _camera;
    private Vector3 _cameraPos;
    private bool _reachedEnd;
    private Tween _moveAnim;

    private void Awake()
    {
        _camera = Camera.main;
    }

    public void IsReachedEnd(bool state)
    {
        _reachedEnd = state;
        if (_reachedEnd)
        {
            _moveAnim?.Kill();
        }
    }

    private void Start()
    {
        EventManager.Instance.OnPlayerMoved?.AddListener(OnPlayerMoved);
        Invoke(nameof(LateStart), 0);
    }

    private void LateStart()
    {
        EventManager.Instance.OnWin.AddListener(() => IsReachedEnd(true));
    }

    private void OnDrawGizmos()
    {
        Camera mainCam = Camera.main;
        if (mainCam == null) return;

        float height = 2f * mainCam.orthographicSize;
        float width = height * mainCam.aspect;
        Vector3 cameraPos = mainCam.transform.position;
        Vector3 screenSize = Vector3.zero;
        screenSize.Set(width - 2, height - 2, 0f);
        cameraPos.z = 0;

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(cameraPos, screenSize);
    }

    private void OnPlayerMoved(Vector3 pos)
    {
        if (_reachedEnd)
        {
            return;
        }

        float height = 2f * _camera.orthographicSize;
        float width = height * _camera.aspect;
        Vector3 screenSize = Vector3.zero;
        screenSize.Set(width - 2, height - 2, 0f);

        bool isOnScreen = _camera.transform.position.x >= pos.x - screenSize.x / 2 &&
                          _camera.transform.position.x <= pos.x + screenSize.x / 2 &&
                          _camera.transform.position.y >= pos.y - screenSize.y / 2 &&
                          _camera.transform.position.y <= pos.y + screenSize.y / 2;

        if (isOnScreen) return;

        Vector3 direction = (pos - _camera.transform.position).normalized;
        Vector3 directionVector = new(Mathf.Abs(direction.x) > Mathf.Abs(direction.y) ? direction.x : 0f,
            Mathf.Abs(direction.x) > Mathf.Abs(direction.y) ? 0f : direction.y, 0f);
        direction = directionVector.normalized;
        Vector3 targetPosition = _camera.transform.position + Utils.Multiply(direction, screenSize);
        _moveAnim = _camera.transform.DOMove(targetPosition, 0.1f);
    }
}