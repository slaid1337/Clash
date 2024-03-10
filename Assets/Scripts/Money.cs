using TMPro;
using UnityEngine;


public class Money : Singletone<Money>
{
    [SerializeField] private TMP_Text[] _moneyTexts;
    private int _money;

    public int money
    {
        get
        {
            return _money;
        }
        set
        {
            _money = value;
        }
    }

    private void Start()
    {
        _money = SaveSystem.GetMoney();
        Refresh(_money);

        SaveSystem.OnMoneyChenged += Refresh;
    }

    public void Refresh(int money)
    {
        _money = money;

        foreach (var item in _moneyTexts)
        {
            item.text = _money.ToString();
        }
    }
}
