using System;
using Grid;
using UnityEngine;

namespace Creators
{
    public class CellCreator : MonoBehaviour
    {
        [SerializeField] protected Cell _cellToSpawn;
        [SerializeField] protected Color _gizmoColor = Color.white;

        private Transform _cellCreatorTransform;

        protected Transform cellCreatorTransform
        {
            get
            {
                if (_cellCreatorTransform == null)
                {
                    _cellCreatorTransform = transform;
                }
                return _cellCreatorTransform;
            }
        }

        protected virtual void Start()
        {
            Invoke(nameof(LateStart), 0);
        }

        protected virtual void LateStart()
        {
            SetTile(_cellToSpawn);
        }

        protected virtual void SetTile(Cell cell)
        {
            EventManager.Instance.OnChangeCell?.Invoke(cellCreatorTransform.position, cell);
        }

        protected virtual void OnDrawGizmos()
        {
            Gizmos.color = _gizmoColor;
            Gizmos.DrawCube(cellCreatorTransform.position, Vector3.one * 0.5f);
        }
    }
}