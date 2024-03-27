using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class BuildMapProject : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private string _name;
    [SerializeField] private Tilemap _tilemap;
    [SerializeField] private Tile _grassTile;
    [SerializeField] private BuildingProjectPanel _buildingProjectPanel;
    public Vector3Int Position;

    private void Start()
    {
        if (SaveSystem.CheckOnBuidProject(_name))
        {
            _tilemap.SetTile(BuildingGrid.Instance.Grid.WorldToCell(transform.position), _grassTile);
            Destroy(gameObject);
        }
        else
        {
            GridBuildingSystem.Instance.AddOccupedField(Position);
        }
    }

    public void Build()
    {
        _tilemap.SetTile(BuildingGrid.Instance.Grid.WorldToCell(transform.position), _grassTile);
        
        BuildingSpawner.Instance.SpawnNewBuild(_buildingProjectPanel.BuildingObject, new Vector3Int[] { Position }, BuildingGrid.Instance.Grid.LocalToCell(transform.position));

        SaveSystem.SetBuildProject(_name);

        GridBuildingSystem.Instance.RemoveOccupedField(Position);

        Destroy(gameObject);
    }

    public void OpenUI()
    {
        _buildingProjectPanel.RectTransform.DOPause();
        _buildingProjectPanel.RectTransform.DOScaleY(1f, 0.8f).SetEase(Ease.OutBounce);
    }

    public void CloseUI(float longest = 0.2f)
    {
        _buildingProjectPanel.RectTransform.DOPause();
        _buildingProjectPanel.RectTransform.DOScaleY(0f, longest);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OpenUI();
    }
}
