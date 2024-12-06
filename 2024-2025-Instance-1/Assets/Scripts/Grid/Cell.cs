using UnityEngine;
using UnityEngine.Tilemaps;


namespace Grid
{
    public class Cell : Tile
    {
        [HideInInspector] public GridManager gridManager;
        public CellObjectBase objectOnCell { get; private set; }

        private void OnEnable()
        {
            if (gameObject != null)
            {
                objectOnCell = gameObject.GetComponent<CellObjectBase>();
                objectOnCell.SetCell(this);
            }
        }

        public void UpdateCellRef(GameObject go)
        {
            if (go != null)
            {
                objectOnCell = go.GetComponent<CellObjectBase>();
                objectOnCell.SetCell(this);
            }
        }
    }   
}

