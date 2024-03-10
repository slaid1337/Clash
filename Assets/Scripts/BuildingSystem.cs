using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class BuildingSystem : Singletone<BuildingSystem>
{
    private Building _currentBuilding;
    [SerializeField] private RectTransform _mainButtons;
    [SerializeField] private RectTransform _buildButtons;
    [SerializeField] private Button _buildButton;
    [SerializeField] private Button _breakButton;
    [SerializeField] private BuildShopMenu _shopMenu;

    public void BreakBuilding()
    {
        _currentBuilding.StopBuilding();
        StopBuilding();
    }

    public void AcceptBuilding()
    {
        _currentBuilding.StopBuilding(_currentBuilding.Position);
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

    public void NewBuilding(GameObject prefab, BuildingObject buildingObject)
    {
        _buildButton.onClick.RemoveAllListeners();
        _breakButton.onClick.RemoveAllListeners();
        _buildButton.onClick.AddListener(AcceptNewBuilding);
        _breakButton.onClick.AddListener(BreakNewBuilding);

        Building building = Instantiate(prefab).GetComponent<Building>();
        building.UpdateBuildingObject(buildingObject);
        OpenBuildButtons();
        _currentBuilding = building;
        building.NewMove();
        building.GetComponent<DragAndDrop>().StartMove(newMove: true);
        _shopMenu.ClosePanel();
    }

    public void AcceptNewBuilding()
    {
        AcceptBuilding();
    }

    public void BreakNewBuilding()
    {
        print(_currentBuilding);
        Destroy(_currentBuilding.gameObject);
        _currentBuilding = null;
        CloseBuildButtons();
        GridBuildingSystem.Instance.HideBuildingGreed();
    }

    public void OpenBuildButtons()
    {
        Sequence sequence = DOTween.Sequence();

        sequence.Append(_mainButtons.DOAnchorPosY(-_mainButtons.sizeDelta.y, 0.2f));
        sequence.Append(_buildButtons.DOAnchorPosY(0f, 0.3f));
    }

    public void CloseBuildButtons()
    {
        Sequence sequence = DOTween.Sequence();

        sequence.Append(_buildButtons.DOAnchorPosY(-_buildButtons.sizeDelta.y, 0.2f));
        sequence.Append(_mainButtons.DOAnchorPosY(0f, 0.3f));
    }
}
