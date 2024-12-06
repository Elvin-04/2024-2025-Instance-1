using Grid;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CreateDoorBtn : ObjectCreator
{
    [SerializeField] private TileBase _door;
    [SerializeField] private TileBase _btn;
    [SerializeField] private List<Transform> _doorTransforms;
    [SerializeField] private Transform _btnTransform;

    private void Start()
    {
        Cell btnCell = CreatCellObject(_gridManager, _btn, _btnTransform);
        CreatCellObjects(_gridManager, _door, _doorTransforms);

        DoorButton btn = btnCell.objectOnCell.GetComponent<DoorButton>();

        btn.doorTransforms = _doorTransforms;
        btnCell.gridManager = _gridManager;
    }
}
