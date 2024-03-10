using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class BuildShopMenu : MonoBehaviour
{
    [SerializeField] private Image _background;
    private RectTransform _rectTransform;
    [SerializeField] private Transform _shopContent;
    [SerializeField] private GameObject _itemPrefab;
    [SerializeField] private ShopItems _items;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _rectTransform.DOAnchorPosX((Screen.width / 2) + (_rectTransform.sizeDelta.x / 2), 0f);

        foreach (var item in _items.shopItems)
        {
            ShopCard card = Instantiate(_itemPrefab, _shopContent).GetComponent<ShopCard>();

            card.ShopItem = item;

            card.BuildingObject = _items.buildingObjects.objects[5];

            card.Refresh();
        }
    }
    
    public void OpenPanel()
    {
        _rectTransform.DOAnchorPosX(0f, 0.7f);
        _background.DOColor(new Color(0, 0, 0, 0.8f), 0.6f);
    }

    public void ClosePanel()
    {
        _rectTransform.DOAnchorPosX((Screen.width / 2) + (_rectTransform.sizeDelta.x / 2), 0.3f);
        _background.DOColor(new Color(0, 0, 0, 0), 0.3f);
    }


}
