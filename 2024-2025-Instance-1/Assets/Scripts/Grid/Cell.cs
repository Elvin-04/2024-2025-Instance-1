using System;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Grid
{
    public class Cell : AnimatedTile
    {
        [SerializeField] private GameObject _prefab;

        public GameObject getPrefab => _prefab;
    }
}