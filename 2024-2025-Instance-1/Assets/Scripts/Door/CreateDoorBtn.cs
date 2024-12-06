using Grid;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CreateDoorBtn : MonoBehaviour
{
    [SerializeField] private GridManager _gridManager;
    private Tilemap _tilemap;
    [SerializeField] private TileBase _door;
    [SerializeField] private TileBase _btn;
    [SerializeField] private Transform _doorTransform;
    [SerializeField] private Transform _btnTransform;

    private void Start()
    {
        _tilemap = _gridManager.tilemap;
        _tilemap.SetTile(_tilemap.WorldToCell(_doorTransform.position), _door);
        _tilemap.SetTile(_tilemap.WorldToCell(_btnTransform.position), _btn);

        GameObject goDoor = _tilemap.GetInstantiatedObject(_tilemap.WorldToCell(_doorTransform.position));

        GameObject goBtn = _tilemap.GetInstantiatedObject(_tilemap.WorldToCell(_btnTransform.position));
        DoorButton btn = goBtn.GetComponent<DoorButton>();

        Cell btnCell = _gridManager.GetCell(_btnTransform.position);
        Cell doorCell = _gridManager.GetCell(_doorTransform.position);
        btnCell.UpdateCellRef(goBtn);
        doorCell.UpdateCellRef(goDoor);

        btn.doorTransform = _doorTransform;
        btnCell.gridManager = _gridManager;
    }
}
