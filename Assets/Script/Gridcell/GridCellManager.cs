using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridCellManager : MonoBehaviour
{
    public static GridCellManager instance;

    [SerializeField]
    private Tilemap tileMap;
    [SerializeField] 
    private List<Vector3Int> tileLocations = new List<Vector3Int>();
    [SerializeField]
    private Dictionary<Vector3Int, GameObject> placedCell = new Dictionary<Vector3Int, GameObject>(); 

    private void Awake()
    {
        if (instance != this && instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int cellPos = tileMap.WorldToCell(mousePos);
            Debug.Log(cellPos);
        }
    }

    private void Start()
    {
        //GetMoveAbleCell();
    }

    public void SetMap(Tilemap tilemap)
    {
        this.tileMap = tilemap;
        GetMoveAbleCell();
    }

    public void SetPlacedCell(Vector3Int placedCell, GameObject obj)
    {
        this.placedCell.Add(placedCell, obj);
    }

    public void RemovePlacedCell(Vector3Int placedCell)
    {
        this.placedCell.Remove(placedCell);
    }

    public GameObject GetPlacedObj(Vector3Int placedCell)
    {
        return this.placedCell[placedCell];
    }

    public bool IsPlacedCell(Vector3Int placedCell)
    {
        if (this.placedCell.ContainsKey(placedCell))
        {
            return true;
        }
        return false;
    }

    private void GetMoveAbleCell()
    {
        for (int x = tileMap.cellBounds.xMin; x < tileMap.cellBounds.xMax; x++)
        {
            for (int y = tileMap.cellBounds.yMin; y < tileMap.cellBounds.yMax; y++)
            {
                Vector3Int localLocation = new Vector3Int(
                    x: x,
                    y: y,
                    z: 0);
                if (tileMap.HasTile(localLocation))
                {
                    tileLocations.Add(localLocation);
                }
            }
        }
    }
    public bool IsPlaceableArea(Vector3Int mouseCellPos)
    {
        if (tileMap.GetTile(mouseCellPos) == null)
        {
            return false;
        }
        return true;
    }

    public List<Vector3Int>  GetCellsPosition()
    {
        return tileLocations;
    }

    public Vector3Int GetObjCell(Vector3 position)
    {
        Vector3Int cellPosition = tileMap.WorldToCell(position);
        return cellPosition;
    }

    public Vector3 PositonToMove(Vector3Int cellPosition)
    {
        return tileMap.GetCellCenterWorld(cellPosition);
    }
}
