using Grid;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CreateDoorBtn : ObjectCreator
{
    [SerializeField] private Cell _door;
    [SerializeField] private Cell _btn;
    [SerializeField] private List<Transform> _doorTransforms;
    [SerializeField] private Transform _btnTransform;

    private void Start()
    {
        CreatCellObject(_gridManager, _btn, _btnTransform);
        CreatCellObjects(_gridManager, _door, _doorTransforms);

        Cell cell = _gridManager.GetCell(_btnTransform.position);
        GameObject go = _gridManager.tilemap.GetInstantiatedObject(_gridManager.tilemap.WorldToCell(_btnTransform.position));
        cell.UpdateCellRef(go);
        DoorButton btn = cell.objectOnCell?.GetComponent<DoorButton>();

        btn.doorTransforms = _doorTransforms;
        cell.gridManager = _gridManager;
    }
}
