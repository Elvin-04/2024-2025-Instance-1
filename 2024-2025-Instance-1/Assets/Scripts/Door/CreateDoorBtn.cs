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
}