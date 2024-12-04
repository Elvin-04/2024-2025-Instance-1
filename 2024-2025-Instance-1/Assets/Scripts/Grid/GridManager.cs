using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Grid
{
    public class GridManager : MonoBehaviour
    {
        [SerializeField] private Tilemap _tilemap;
        public Cell[,] Cells { get; private set; }

        private void Awake()
        {
            Cells = new Cell[_tilemap.size.x, _tilemap.size.y];
            
            Debug.Log("size : " + _tilemap.size);

            for (int x = 0; x < _tilemap.size.x; x++)
            {
                for (int y = 0; y < _tilemap.size.y; y++)
                {
                    Vector3Int tilePosition = new(x, y, 0);
                    Debug.Log(_tilemap.);
                    //Cells[x, y] = _tilemap.GetInstantiatedObject(tilePosition).GetComponent<Cell>();
                }
            }
        }
    } 
}