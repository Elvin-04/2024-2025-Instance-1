using UnityEngine.Tilemaps;

namespace Grid
{
    public class Cell : Tile
    {
        public CellObjectBase objectOnCell { get; private set; }

        private void OnEnable()
        {
            if (gameObject != null) objectOnCell = gameObject.GetComponent<CellObjectBase>();
        }
    }
}