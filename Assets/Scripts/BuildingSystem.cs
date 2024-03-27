using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;

public class BuildingSystem : Singletone<BuildingSystem>
{
    private Building _currentBuilding;
    [SerializeField] private RectTransform _mainButtons;
    [SerializeField] private RectTransform _buildButtons;
    [SerializeField] private QuestPanel _questMenu;
    [SerializeField] private Button _buildButton;
    [SerializeField] private Button _breakButton;
    [SerializeField] private BuildShopMenu _shopMenu;

    public UnityEvent<string> OnBuildNewBuilding;

    public void BreakBuilding()
    {
        _currentBuilding.StopBuilding();
        StopBuilding();
    }

    public void AcceptBuilding()
    {
        _currentBuilding.AcceptMove(_currentBuilding.Position);
        StopBuilding();
    }

    public void DisableBuildButton()
    {
        _buildButton.interactable = false;
    }

    public void EnableBuildButton()
    {
        _buildButton.interactable = true;
    }

    public void StartMoveBuilding(Building building)
    {
        _currentBuilding = building;

        _buildButton.onClick.RemoveAllListeners();
        _breakButton.onClick.RemoveAllListeners();
        _buildButton.onClick.AddListener(AcceptBuilding);
        _breakButton.onClick.AddListener(BreakBuilding);

        OpenBuildButtons();
    }

    public void StopBuilding()
    {
        CloseBuildButtons();
    }

    public void NewBuilding(GameObject prefab, BuildingObject buildingObject, int cost, int lvl)
    {
        _buildButton.onClick.RemoveAllListeners();
        _breakButton.onClick.RemoveAllListeners();
        _buildButton.onClick.AddListener(delegate { AcceptNewBuilding(cost); SaveSystem.AddLvl(lvl); });
        _breakButton.onClick.AddListener(BreakNewBuilding);

        Building building = Instantiate(prefab).GetComponent<Building>();
        building.UpdateBuildingObject(buildingObject);
        OpenBuildButtons();
        _currentBuilding = building;
        building.NewMove();
        building.GetComponent<DragAndDrop>().StartMove(newMove: true);
        _shopMenu.ClosePanel();
    }

    public void NewBuildingProject(GameObject prefab, BuildingObject buildingObject, int cost)
    {
        _buildButton.onClick.RemoveAllListeners();
        _breakButton.onClick.RemoveAllListeners();
        _buildButton.onClick.AddListener(delegate { AcceptNewBuilding(cost); });
        _breakButton.onClick.AddListener(BreakNewBuilding);

        Building building = Instantiate(prefab).GetComponent<Building>();
        building.UpdateBuildingObject(buildingObject);
        OpenBuildButtons();
        _currentBuilding = building;
        _shopMenu.ClosePanel();
    }

    public void AcceptNewBuilding(int cost)
    {
        SaveSystem.SpendMoney(cost);
        _currentBuilding.AcceptNewMove(_currentBuilding.Position);
        OnBuildNewBuilding?.Invoke(_currentBuilding.Name);
        StopBuilding();
    }

    public void BreakNewBuilding()
    {
        Destroy(_currentBuilding.gameObject);
        _currentBuilding = null;
        CloseBuildButtons();
        GridBuildingSystem.Instance.HideBuildingGreed();
    }

    public void OpenBuildButtons()
    {
        Sequence sequence = DOTween.Sequence();

        _questMenu.ClosePanel();

        sequence.Append(_mainButtons.DOAnchorPosY(-_mainButtons.sizeDelta.y, 0.2f));
        sequence.Append(_buildButtons.DOAnchorPosY(0f, 0.3f));
    }

    public void CloseBuildButtons()
    {
        Sequence sequence = DOTween.Sequence();

        sequence.Append(_buildButtons.DOAnchorPosY(-_buildButtons.sizeDelta.y, 0.2f));
        sequence.Append(_mainButtons.DOAnchorPosY(0f, 0.3f));
    }

    public List<Building> GetSpawnedBuildingsByName(string name)
    {
        List<Building> buildings = FindObjectsByType<Building>(FindObjectsInactive.Include, FindObjectsSortMode.None).ToList();
        List<Building> result = new List<Building>();

        foreach (Building b in buildings)
        {
            if (b.Name == name)
            {
                result.Add(b);
            }
        }

        return result;
    }
}
