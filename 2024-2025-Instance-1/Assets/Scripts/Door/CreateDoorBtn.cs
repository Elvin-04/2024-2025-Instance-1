using System.Collections.Generic;
using Grid;
using UnityEngine;

public class CreateDoorBtn : ObjectCreator
{
    [SerializeField] private Cell _door;
    [SerializeField] private Cell _btn;
    [SerializeField] private List<Transform> _doorTransforms;
    [SerializeField] private Transform _btnTransform;

    private void Start()
    {
        CreateCell(_gridManager, _btn, _btnTransform);
        _gridManager.GetObjectsOnCell(_btnTransform.position).ForEach(objectOnCell =>
            (objectOnCell as DoorButton)?.SetDoorTransforms(_doorTransforms));

        CreateCells(_gridManager, _door, _doorTransforms);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawCube(_btnTransform.position, Vector3.one * 0.5f);
        foreach(Transform t in _doorTransforms)
        {
            Gizmos.color = Color.white;
            Gizmos.DrawCube(t.position, Vector3.one);
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(t.position, _btnTransform.position);
        }
    }
}