using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class BuildingPanel : MonoBehaviour
{
    public Button RotateButton;
    public Button MoveButton;
    public TMP_Text _moneyText;
    public TMP_Text _collectMoneyText;

    public RectTransform RectTransform
    {
        get { return GetComponent<RectTransform>(); }
    }

    private void Start()
    {
        RectTransform.DOScaleY(0f, 0f);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!IsPointerOverUIObject())
            {
                transform.GetComponent<RectTransform>().DOPause();
                transform.GetComponent<RectTransform>().DOScaleY(0f, 0.2f);
            }
        }
    }

    public void UpdateInfo(int moneyPerSecond, int currentMoney, int capacity)
    {
        _moneyText.text = moneyPerSecond.ToString() + "/" + Lean.Localization.LeanLocalization.GetTranslationText("Min");
        _collectMoneyText.text = currentMoney.ToString() + "/" + capacity.ToString();
    }

    public static bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        foreach (RaycastResult r in results)
            if (r.gameObject.GetComponent<RectTransform>() != null)
                return true;

        return false;
    }
}
