using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Grid
{
    public class Cell : AnimatedTile
    {
        [SerializeField] private GameObject _prefab;

        public GameObject Getprefab => _prefab;

    }
}