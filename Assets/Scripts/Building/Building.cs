using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class Building : MonoBehaviour, IPointerClickHandler
{
    public Vector3Int StartPosition;
    public Vector3Int Position;
    public Vector2Int Bounds;
    [SerializeField] private BuildingPanel _panel;
    [SerializeField] private SpriteRenderer _buildSprite;
    private DragAndDrop _dragAndDrop;

    [SerializeField] private BuildingObjects _buildingObjects;
    [SerializeField] private GameObject _flag;
    private BuildingObject _buildingObject;
    private float _currentMoney;
    private float _moneyPerSecond;
    public string Name;

    public float Money
    {
        get
        {
            return _currentMoney;
        }
    }

    public DragAndDrop DragAndDrop { get { return _dragAndDrop; } }

    private void Start()
    {
        
    }

    public void Rotate()
    {
        _buildSprite.flipX = !_buildSprite.flipX;
    }

    public void UpdateBuildingObject(BuildingObject buildingObject)
    {
        _buildingObject = buildingObject;
    }

    public void Move()
    {
        _dragAndDrop = GetComponent<DragAndDrop>();
        BuildingSystem.Instance.StartMoveBuilding(this);
        _dragAndDrop.CanDrag = true;
        _dragAndDrop.StartMove();
        CloseUI();
    }

    public void NewMove()
    {
        _dragAndDrop = GetComponent<DragAndDrop>();
        _dragAndDrop.CanDrag = true;
        _dragAndDrop.StartMove(newMove: true);
        CloseUI(longest:0f);
    }

    public void AcceptMove(Vector3Int position)
    {
        _dragAndDrop.CanDrag = false;
        GridBuildingSystem.Instance.RemoveOccupedField(GetPositions().ToArray());
        StartPosition = position;
        GridBuildingSystem.Instance.AddOccupedField(GetPositions().ToArray());
        UpdatePosition(StartPosition);
        BuildingSpawner.Instance.SaveBuildings();
        GridBuildingSystem.Instance.HideBuildingGreed();
    }

    public void AcceptNewMove(Vector3Int position)
    {
        _dragAndDrop.CanDrag = false;

        StartPosition = position;
        UpdatePosition(StartPosition);
        BuildingSpawner.Instance.AddBuilding(this, _buildingObject, GetPositions().ToArray(), StartPosition);
        GridBuildingSystem.Instance.HideBuildingGreed();
    }

    public void StopBuilding()
    {
        _dragAndDrop.CanDrag = false;
        UpdatePosition(StartPosition);
        GridBuildingSystem.Instance.HideBuildingGreed();
    }

    public void UpdatePosition()
    {
        Position = GridBuildingSystem.Instance.Grid.WorldToCell(transform.position);
    }

    public void UpdatePosition(Vector3Int position)
    {
        Position = position;
        transform.position = GridBuildingSystem.Instance.Grid.CellToLocal(Position);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OpenUI();
    }

    public void OpenUI()
    {
        if (_dragAndDrop.CanDrag) return;
        _panel.RectTransform.DOPause();
        _panel.RectTransform.DOScaleY(1f, 0.8f).SetEase(Ease.OutBounce);
        _panel.UpdateInfo(_buildingObject.Income,(int) _currentMoney, _buildingObject.Capacity);
    }

    public void CloseUI(float longest = 0.2f)
    {
        _panel.RectTransform.DOPause();
        _panel.RectTransform.DOScaleY(0f, longest);
    }

    public void CollectMoney()
    {
        SaveSystem.AddMoney((int)_currentMoney);
        _panel.UpdateInfo(_buildingObject.Income, (int)_currentMoney, _buildingObject.Capacity);
        _flag.SetActive(false);
        _currentMoney = 0;

        BuildingSpawner.Instance.SaveBuildings();
    }
    
    public void OnLoad(BuildingSave save)
    {
        _dragAndDrop = GetComponent<DragAndDrop>();

        _panel.RotateButton.onClick.AddListener(Rotate);
        _panel.MoveButton.onClick.AddListener(Move);

        _flag.GetComponent<Button>().onClick.AddListener(CollectMoney);

        foreach (var item in _buildingObjects.objects)
        {
            if (item.Name == save.Name)
            {
                _buildingObject = item;
                break;
            }
        }

        _moneyPerSecond = (float)_buildingObject.Income / 60;

        _currentMoney = save.Money;
        StartPosition = save.SpawnPosition;
        Position = StartPosition;

        Vector3 newPos = GridBuildingSystem.Instance.Grid.GetCellCenterWorld(StartPosition);

        transform.position = new Vector3(newPos.x, newPos.y, 0f);

        StartCoroutine(AddMoney(_moneyPerSecond));
    }

    public void OnLoad(string name)
    {
        _dragAndDrop = GetComponent<DragAndDrop>();

        _panel.RotateButton.onClick.AddListener(Rotate);
        _panel.MoveButton.onClick.AddListener(Move);

        _flag.GetComponent<Button>().onClick.AddListener(CollectMoney);

        foreach (var item in _buildingObjects.objects)
        {
            if (item.Name == name)
            {
                _buildingObject = item;
                break;
            }
        }

        _moneyPerSecond = (float) _buildingObject.Income / 60;

        _currentMoney = 0;
        StartPosition = GridBuildingSystem.Instance.Grid.LocalToCell(transform.position);
        Position = StartPosition;

        StartCoroutine(AddMoney(_moneyPerSecond));
    }

    private IEnumerator AddMoney(float moneyPerSecond)
    {
        yield return new WaitForSeconds(1f);

        if (_currentMoney + moneyPerSecond <= _buildingObject.Capacity)
        {
            _currentMoney += moneyPerSecond;
        }
        else
        {
            _currentMoney = _buildingObject.Capacity;
        }

        if (_currentMoney >= _buildingObject.Capacity / 4) _flag.SetActive(true);

        _panel.UpdateInfo(_buildingObject.Income,(int) _currentMoney, _buildingObject.Capacity);
        StartCoroutine(AddMoney(moneyPerSecond));
    }

    public List<Vector3Int> GetPositions()
    {

        List<Vector3Int> positions = new List<Vector3Int>();

        for (int i = 0; i < Bounds.x; i++)
        {
            for (int j = 0; j < Bounds.y; j++)
            {
                Vector3Int newPosition = new Vector3Int(StartPosition.x + i, StartPosition.y + j, StartPosition.z);
                positions.Add(newPosition);
            }
        }

        return positions;
    }
}
