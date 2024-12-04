using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Grid
{
    public class Cell : MonoBehaviour
    {
        [SerializeField] private CellObject _objectOnCell;
    }
}