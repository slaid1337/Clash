using Eiko.YaSDK;
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

    public override void Awake()
    {
        base.Awake();

        GameLoader.Instance.OnLoad.AddListener(OnLoad);
    }

    public  void OnLoad()
    {
        _money =  SaveSystem.GetMoney();
        Refresh(_money);

        SaveSystem.OnMoneyChenged += Refresh;
    }

    [ContextMenu("addMOney")]
    public void AddMoney()
    {
        SaveSystem.AddMoney(20000);
    }

    public void Refresh(int money)
    {
        _money = money;

        foreach (var item in _moneyTexts)
        {
            item.text = _money.ToString();
        }
    }

    public void GetMoneyForReward()
    {
        YandexSDK.instance.onRewardedAdReward += OnReward;
        YandexSDK.instance.onRewardedAdError += OnRewardError;
        YandexSDK.instance.onRewardedAdClosed += OnRewardClose;

        YandexSDK.instance.ShowRewarded("");
    }

    public void OnReward(string str)
    {
        YandexSDK.instance.onRewardedAdReward -= OnReward;
        YandexSDK.instance.onRewardedAdError -= OnRewardError;
        YandexSDK.instance.onRewardedAdClosed -= OnRewardClose;

        SaveSystem.AddMoney(50);
    }

    public void OnRewardError(string str)
    {
        YandexSDK.instance.onRewardedAdReward -= OnReward;
        YandexSDK.instance.onRewardedAdError -= OnRewardError;
        YandexSDK.instance.onRewardedAdClosed -= OnRewardClose;
    }
    public void OnRewardClose(int str)
    {
        YandexSDK.instance.onRewardedAdReward -= OnReward;
        YandexSDK.instance.onRewardedAdError -= OnRewardError;
        YandexSDK.instance.onRewardedAdClosed -= OnRewardClose;
    }
}
