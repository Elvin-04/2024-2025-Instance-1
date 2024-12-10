using UnityEngine;
using UnityEngine.Tilemaps;

namespace Grid
{
    public class Cell : AnimatedTile
    {
        [SerializeField] private GameObject _prefab;
        public GameObject instancedObject { get; private set; }

        public GameObject getPrefab => _prefab;

        public void SetInstancedObject(GameObject obj)
        {
            instancedObject = obj;
        }
    }
}