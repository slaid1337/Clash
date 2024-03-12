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


    public void Refresh()
    {
        _buyBtn.onClick.AddListener(
            delegate 
            { 
                BuildingSystem.Instance.NewBuilding(BuildingObject.Prefab, BuildingObject, ShopItem.Cost);
            });

        _image.sprite = ShopItem.ActiveSprite;
        _costText.text = ShopItem.Cost.ToString();
        _countText.text = "0/" + ShopItem.LevelOpens[0].Count.ToString();
    }
}
