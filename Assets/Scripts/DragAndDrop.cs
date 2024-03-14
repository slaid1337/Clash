using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragAndDrop : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private Vector2Int _bounds;
    private bool _canDrag;

    public bool CanDrag
    {
        get { return _canDrag; }
        set { _canDrag = value; }
    }

    public void StartMove(bool newMove = false)
    {
        GridBuildingSystem.Instance.ShowBuildingGreed();
        _bounds = GetComponent<Building>().Bounds;
        UpdateDrag();

        if (newMove)
        {
            int x = Screen.width / 2;
            int y = Screen.height / 2;

            Vector3 screenPos = new Vector3(x, y, 0);

            Vector3Int pos = GetComponent<Building>().Position;

            TileType tileType = TileType.Green;

            if (!GridBuildingSystem.Instance.CheckMapSize(pos)) tileType = TileType.Red;

            Vector3 newPos = Camera.main.ScreenToWorldPoint(screenPos);

            transform.position = new Vector3(newPos.x, newPos.y, 0f);
            for (int i = -1; i < _bounds.x + 1; i++)
            {
                for (int j = -1; j < _bounds.y + 1; j++)
                {
                    if (!GridBuildingSystem.Instance.CheckMapSize(new Vector3Int(pos.x + i, pos.y + j)))
                    {
                        GridBuildingSystem.Instance.UpdateTile(new Vector3Int(pos.x + i, pos.y + j), TileType.None);
                    }
                    else
                    {
                        GridBuildingSystem.Instance.UpdateTile(new Vector3Int(pos.x + i, pos.y + j), TileType.Default);
                    }

                }
            }

            GetComponent<Building>().UpdatePosition();

            pos = GetComponent<Building>().Position;
            for (int i = -1; i < _bounds.x + 1; i++)
            {
                for (int j = -1; j < _bounds.y + 1; j++)
                {
                    GridBuildingSystem.Instance.UpdateTile(new Vector3Int(pos.x + i, pos.y + j), tileType);
                }
            }
        }    
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        /*if (!_canDrag) return;
        GridBuildingSystem.Instance.ShowBuildingGreed();
        _bounds = GetComponent<Building>().Bounds;*/
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!_canDrag) return;

        UpdateDrag();
    }

    public void UpdateDrag()
    {
        Vector3Int pos = GetComponent<Building>().Position;

        TileType tileType = TileType.Green;

        BuildingSystem.Instance.EnableBuildButton();

        if (!GridBuildingSystem.Instance.CheckMapSize(pos))
        {
            tileType = TileType.Red;
            BuildingSystem.Instance.DisableBuildButton();
        }
        List<Vector3Int> positions = new List<Vector3Int>();

        for (int i = 0; i < _bounds.x; i++)
        {
            for (int j = 0; j < _bounds.y; j++)
            {
                Vector3Int newPosition = new Vector3Int(pos.x + i, pos.y + j, pos.z);
                positions.Add(newPosition);
            }
        }
        print(pos);
        if (pos != GetComponent<Building>().StartPosition && GridBuildingSystem.Instance.CheckOccupation(positions.ToArray()))
        {
            tileType = TileType.Red;
            BuildingSystem.Instance.DisableBuildButton();
        }

        Vector3 newPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        transform.position =  new Vector3(newPos.x, newPos.y, 0f);
        for (int i = -1; i < _bounds.x + 1; i++)
        {
            for (int j = -1; j < _bounds.y + 1; j++)
            {
                if (!GridBuildingSystem.Instance.CheckMapSize(new Vector3Int(pos.x + i, pos.y + j)))
                {
                    GridBuildingSystem.Instance.UpdateTile(new Vector3Int(pos.x + i, pos.y + j), TileType.None);
                }
                else
                {
                    GridBuildingSystem.Instance.UpdateTile(new Vector3Int(pos.x + i, pos.y + j), TileType.Default);
                }
                
            }
        }

        GetComponent<Building>().UpdatePosition();

        pos = GetComponent<Building>().Position;
        for (int i = -1; i < _bounds.x + 1; i++)
        {
            for (int j = -1; j < _bounds.y + 1; j++)
            {
                GridBuildingSystem.Instance.UpdateTile(new Vector3Int(pos.x + i, pos.y + j), tileType);
            }
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        /*if (!_canDrag) return;

        GridBuildingSystem.Instance.HideBuildingGreed();*/
    }
}
