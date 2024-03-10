using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;
using static UnityEditor.PlayerSettings;

[ExecuteInEditMode]
public class GridBuildingSystem : Singletone<GridBuildingSystem>
{
    public Grid Grid;
    [SerializeField] private Tilemap _map;
    [SerializeField] private Tile _defaultTile;
    [SerializeField] private Tile _GreenTile;
    [SerializeField] private Tile _RedTile;
    [SerializeField] private MapBounds _mapBounds;
    public List<Vector3Int> OccupedFields { get; private set; }

    private void Start()
    {
        _map.ClearAllTiles();
        OccupedFields = SaveSystem.GetOccupedFields();
        print(OccupedFields);
    }

    public void ShowBuildingGreed()
    {
        UpdateMap();
    }

    public void HideBuildingGreed()
    {
        _map.ClearAllTiles();
    }

    private void UpdateMap()
    {
        _map.ClearAllTiles();

        _map.SetTile(_mapBounds.TopLeft, _RedTile);
        _map.SetTile(_mapBounds.TopRight, _RedTile);
        _map.SetTile(_mapBounds.BottomLeft, _RedTile);
        _map.SetTile(_mapBounds.BottomRight, _RedTile);

        Vector2 mapSize = new Vector2(Mathf.Abs(_mapBounds.TopLeft.x) + Mathf.Abs(_mapBounds.BottomLeft.x), Mathf.Abs(_mapBounds.TopLeft.y) + Mathf.Abs(_mapBounds.TopRight.y));

        for (int i = _mapBounds.BottomLeft.x; i < mapSize.x - Mathf.Abs(_mapBounds.BottomLeft.x) + 1; i++)
        {
            for (int j = _mapBounds.BottomRight.y; j < mapSize.y - Mathf.Abs(_mapBounds.BottomLeft.y) + 1; j++)
            {
                _map.SetTile(new Vector3Int(i, j), _defaultTile);
            }
        }
    }

    public void UpdateTile(Vector3Int pos, TileType type)
    {
        Tile tile = null;

        switch (type)
        {
            case TileType.Default:
                tile = _defaultTile;
                break;
            case TileType.Green:
                tile = _GreenTile;
                break;
            case TileType.Red:
                tile = _RedTile;
                break;
            case TileType.None:
                tile = null;
                break;
        }

        _map.SetTile(pos, tile);
    }

    public bool CheckMapSize(Vector3Int position)
    {
        if (position.x > _mapBounds.TopLeft.x || position.x < _mapBounds.BottomLeft.x || position.y > _mapBounds.TopLeft.y || position.y < _mapBounds.TopRight.y)
        {
            return false;
        }

        return true;
    }

    public bool CheckOccupation(Vector3Int[] positions)
    {
        foreach (var item in positions)
        {
            if (OccupedFields.Contains(item))
            {
                return true;
            }
        }

        return false;
    }

    private void OnDrawGizmos()
    {
        Vector2 mapSize = new Vector2(Mathf.Abs(_mapBounds.TopLeft.x) + Mathf.Abs(_mapBounds.BottomLeft.x), Mathf.Abs(_mapBounds.TopLeft.y) + Mathf.Abs(_mapBounds.TopRight.y));

        for (int i = _mapBounds.BottomLeft.x; i < mapSize.x - Mathf.Abs(_mapBounds.BottomLeft.x) + 1; i++)
        {
            for (int j = _mapBounds.BottomRight.y; j < mapSize.y - Mathf.Abs(_mapBounds.BottomLeft.y) + 1; j++)
            {
                Vector3Int pos = new Vector3Int(i, j);
                Handles.Label(Grid.GetCellCenterWorld(pos), pos.x + " : " + pos.y);
            }
        }
    }
}

[Serializable]
public struct MapBounds
{
    public Vector3Int TopLeft, TopRight, BottomLeft, BottomRight;
}

public enum TileType
{
    Default,
    Green,
    Red,
    None
}
