using Grid;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CreateDoorBtn : ObjectCreator
{
    [SerializeField] private TileBase _door;
    [SerializeField] private TileBase _btn;
    [SerializeField] private Transform _doorTransform;
    [SerializeField] private Transform _btnTransform;

    private void Start()
    {
        Cell btnCell = CreatCellObject(_gridManager, _btn, _btnTransform);
        CreatCellObject(_gridManager, _door, _doorTransform);

        DoorButton btn = btnCell.objectOnCell.GetComponent<DoorButton>();

        btn.doorTransform = _doorTransform;
        btnCell.gridManager = _gridManager;
    }
}
