using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SettingsPanel : MonoBehaviour
{
    [SerializeField] private Image _background;
    private RectTransform _rectTransform;
    private float _startPosY;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();

        _startPosY = _rectTransform.anchoredPosition.y;
    }

    public void OpenPanel()
    {
        _rectTransform.DOAnchorPosY(0f, 0.7f);
        _background.DOColor(new Color(0, 0, 0, 0.8f), 0.6f);
        _background.raycastTarget = true;
    }

    public void ClosePanel()
    {
        _rectTransform.DOAnchorPosY(_startPosY, 0.3f);
        _background.DOColor(new Color(0, 0, 0, 0), 0.3f);
        _background.raycastTarget = false;
    }
}
