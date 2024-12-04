using System;
using UnityEngine;

namespace Grid
{
    [Serializable]
    public class CellObject 
    {
        public bool IsUnbeatable = false;
        public CellObject()
        {
            Debug.Log("Si je m'affiche c'est bon");
        }
    }
}