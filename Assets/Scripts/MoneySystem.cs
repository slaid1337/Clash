using TMPro;
using UnityEngine;

public class MoneySystem : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;

    private void Start()
    {
        _text.text = SaveSystem.GetMoney().ToString();

        SaveSystem.OnMoneyChenged += RefreshText;
    }

    private void RefreshText(int money)
    {
        _text.text = money.ToString();
    }
}
