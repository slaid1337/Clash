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

    private void Awake()
    {
        if (SaveSystem.CheckOnBuidProject(_name))
        {
            _tilemap.SetTile(BuildingGrid.Instance.Grid.WorldToCell(transform.position), _grassTile);
            Destroy(gameObject);
        }
    }

    public void Build()
    {
        _tilemap.SetTile(BuildingGrid.Instance.Grid.WorldToCell(transform.position), _grassTile);



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
