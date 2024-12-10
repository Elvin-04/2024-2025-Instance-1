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

        public GameObject instancedObject { get; private set; }
        public void SetInstancedObject(GameObject obj) => instancedObject = obj;

        public GameObject Getprefab => _prefab;

    }
}