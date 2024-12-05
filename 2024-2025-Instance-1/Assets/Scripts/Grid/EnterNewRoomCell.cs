using UnityEngine;
using UnityEngine.Tilemaps;

namespace Grid
{
    public class EnterNewRoomCell : CellObjectBase
    {
        [SerializeField] private Vector2Int _previousCamPos;
        [SerializeField] private Vector2Int _nextCamPos;

        private bool _posIsNext = true;

        public Vector2Int nextCamPos
        {
            get
            {
                Vector2Int rv = _posIsNext ? _nextCamPos : _previousCamPos;
                _posIsNext = !_posIsNext;
                return rv;
            }
        }
    }
}