using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopCard : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private GameObject _buyOverlay;
    [SerializeField] private GameObject _shadowOverlay;
    [SerializeField] private TMP_Text _costText;
    [SerializeField] private Button _buyBtn;
    [SerializeField] private TMP_Text _lvlText;
    [SerializeField] private TMP_Text _countText;
    public ShopItem ShopItem;
    public BuildingObject BuildingObject;

    private void Start()
    {
        SaveSystem.OnMoneyChenged += CheckOnBuy;

        _buyBtn.onClick.AddListener(Build);
    }

    public  void Refresh()
    {
        _image.sprite = ShopItem.ActiveSprite;
        _costText.text = ShopItem.Cost.ToString();
        _lvlText.text = ShopItem.LevelOpens[0].Level.ToString();
        CheckOnBuildable( SaveSystem.GetMoney());
    }

    private void Build()
    {
        BuildingSystem.Instance.NewBuilding(BuildingObject.Prefab, BuildingObject, ShopItem.Cost, ShopItem.Level);
    }

    private void OnDestroy()
    {
        SaveSystem.OnMoneyChenged -= CheckOnBuy;
    }

    private void CheckOnBuy(int money)
    {
        CheckOnBuildable(money);
    }

    private void CheckOnBuildable(int money)
    {
        int lvlIndex = 0;
        int lvl = LvlController.Instance.GetCurrentLevel();
        int Count = BuildingSystem.Instance.GetSpawnedBuildingsByName(BuildingObject.Name).Count;

        for (int i = ShopItem.LevelOpens.Length - 1; i > 0; i--)
        {;
            if (lvl >= ShopItem.LevelOpens[i].Level)
            {
                lvlIndex = i;
                
                break;
            }
        }

        _countText.text = Count + "/" + ShopItem.LevelOpens[lvlIndex].Count.ToString();

        if (lvl >= ShopItem.LevelOpens[lvlIndex].Level)
        {
            if (Count < ShopItem.LevelOpens[lvlIndex].Count)
            {
                if (money >= ShopItem.Cost)
                {
                    _buyBtn.interactable = true;
                }
                else
                {
                    _buyBtn.interactable = false;
                }
            }
            else
            {
                _buyBtn.interactable = false;
            }
            _image.sprite = ShopItem.ActiveSprite;
            _shadowOverlay.SetActive(false);
        }
        else
        {
            _image.sprite = ShopItem.DisabledSprite;
            _shadowOverlay.SetActive(true);
        }
    }
}
