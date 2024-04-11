using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System.Collections.Generic;

public class BuildShopMenu : MonoBehaviour
{
    [SerializeField] private Image _background;
    private RectTransform _rectTransform;
    [SerializeField] private Transform _shopContent;
    [SerializeField] private GameObject _itemPrefab;
    [SerializeField] private ShopItems _items;
    private List<ShopCard> _cards;
    private float _startPosX;

    private void Awake()
    {
        GameLoader.Instance.OnLoad.AddListener(OnLoad);
    }

    public void OnLoad()
    {
        _rectTransform = GetComponent<RectTransform>();

        _startPosX = _rectTransform.anchoredPosition.x;

        _cards = new List<ShopCard>();

        foreach (var item in _items.shopItems)
        {
            ShopCard card = Instantiate(_itemPrefab, _shopContent).GetComponent<ShopCard>();

            card.ShopItem = item;

            foreach (var building in _items.buildingObjects.objects)
            {
                if (building.Name == item.Name)
                {
                    card.BuildingObject = building;
                    break;
                }
            }
            _cards.Add(card);
            card.Refresh();
        }
    }
    
    public void OpenPanel()
    {
        _rectTransform.DOAnchorPosX(0f, 0.7f);
        _background.DOColor(new Color(0, 0, 0, 0.8f), 0.6f);

        foreach (var item in _cards)
        {
            item.Refresh();
        }

        _background.raycastTarget = true;
    }

    public void ClosePanel()
    {
        _rectTransform.DOAnchorPosX(_startPosX, 0.3f);
        _background.DOColor(new Color(0, 0, 0, 0), 0.3f);

        _background.raycastTarget = false;
    }
}
