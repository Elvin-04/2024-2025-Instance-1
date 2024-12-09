using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    private static LevelManager _instance;

    [field: SerializeField] public Transform spawnPoint { get; private set; }

    public UnityEvent onWin { get; private set; }

    private void Awake()
    {
        Assert.IsNotNull(spawnPoint, "spawnPoint is null in LevelManager");
    }

    private void Start()
    {
        if (_instance)
        {
            Destroy(gameObject);
            return;
        } 

        _instance = this;
        EventManager.Instance.OnRetry.AddListener(ReloadScene);
    }

    public void Retry()
    {
        Debug.Log("Retry Level");
    }



    public void Win()
    {
        onWin.Invoke();
    }
    public void ReloadScene()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadSceneAsync(scene.name);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.grey;
        Gizmos.DrawCube(spawnPoint.position, Vector3.one);
    }
}
