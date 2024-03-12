using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

public class BuildingProjectPanel : MonoBehaviour
{
    private RectTransform _panel;
    [SerializeField] private ShopItem _shopItem;
    [SerializeField] private BuildingObjects _buildingObjects;
    [SerializeField] private Button _buyButton;
    private BuildingObject _buildingObject;
    [SerializeField] private BuildMapProject _buildingMapProject;
    [SerializeField] private TMP_Text _costText;
    [SerializeField] private TMP_Text _incomeText;
    [SerializeField] private GameObject _blockPanel;
    [SerializeField] private TMP_Text _lvlText;

    public RectTransform RectTransform
    {
        get { return GetComponent<RectTransform>(); }
    }

    private void Start()
    {
        _panel = GetComponent<RectTransform>();
        _panel.DOScaleY(0f, 0f);

        foreach (var item in _buildingObjects.objects)
        {
            if (_shopItem.Name == item.Name)
            {
                _buildingObject = item;
                break;
            }
        }

        _costText.text = _shopItem.Cost.ToString();
        _incomeText.text = _buildingObject.Income.ToString();
        _lvlText.text = "Level " + _shopItem.LevelOpens[0].Level.ToString();
        _buyButton.onClick.AddListener(Build);
        SaveSystem.OnLvlChenged += OnLvlChenge;

        if ( LvlController.Instance.GetCurrentLevel() < _shopItem.LevelOpens[0].Level)
        {
            _buyButton.interactable = false;
            _blockPanel.SetActive(true);
        }
        else
        {
            _buyButton.interactable = true;
            _blockPanel.SetActive(false);
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!BuildingPanel.IsPointerOverUIObject())
            {
                transform.GetComponent<RectTransform>().DOPause();
                transform.GetComponent<RectTransform>().DOScaleY(0f, 0.2f);
            }
        }
    }

    private void OnDestroy()
    {
        SaveSystem.OnLvlChenged -= OnLvlChenge;
    }

    private void OnLvlChenge(int lvl)
    {
        if (LvlController.Instance.GetCurrentLevel() < _shopItem.LevelOpens[0].Level)
        {
            _buyButton.interactable = false;
            _blockPanel.SetActive(true);
        }
        else
        {
            _buyButton.interactable = true;
            _blockPanel.SetActive(false);
        }
    }

    void Build()
    {
        _buildingMapProject.Build();

        BuildingSystem.Instance.NewBuildingProject(_buildingObject.Prefab, _buildingObject, _shopItem.Cost);
    }
}
