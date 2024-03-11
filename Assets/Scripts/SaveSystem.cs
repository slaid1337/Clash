using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Newtonsoft.Json;

public static class SaveSystem
{
    // эвент на изменение денег
    public static Action<int> OnMoneyChenged;
    // эвент на изменение уровня
    public static Action<int> OnLvlChenged;

    public static void SpendMoney(int cost)
    {
        SetMoney(GetMoney() - cost);
    }

    /// <summary>
    /// устанавливает значение денег
    /// </summary>
    /// <param name="money">деньги</param>
    public static void SetMoney(int money)
    {
        PlayerPrefs.SetInt("Money", money);
        OnMoneyChenged?.Invoke(money);
    }

    /// <summary>
    /// добавляет деньги
    /// </summary>
    /// <param name="money">деньги</param>
    public static void AddMoney(int money)
    {
        int newMoney = money + GetMoney();
        PlayerPrefs.SetInt("Money", newMoney);
        OnMoneyChenged?.Invoke(newMoney);
    }

    /// <summary>
    /// получает текущее значение денег
    /// </summary>
    /// <returns>деньги</returns>
    public static int GetMoney()
    {
        return PlayerPrefs.GetInt("Money", 90);
    }

    /// <summary>
    /// проверка на первый запуск
    /// </summary>
    /// <returns></returns>
    public static bool IsFirstStart()
    {
        return PlayerPrefs.GetInt("IsFirstStart", 0) == 0 ? true : false;
    }

    /// <summary>
    /// устанавливает что запуск не первый
    /// </summary>
    public static void SetFirstStart()
    {
        PlayerPrefs.SetInt("IsFirstStart", 1);
    }

    public static List<Vector3Int> GetOccupedFields()
    {
        List<BuildingSave> saves = GetBuildingSaves();
        List<Vector3Int> positions = new List<Vector3Int>();

        if (saves != null)
        {
            foreach (var item in saves)
            {
                positions.Add(item.Position);
            }
        }
        else
        {
            return new List<Vector3Int> ();
        }
        

        return positions;
    }

    public static bool CheckOnBuidProject(string name)
    {
        return PlayerPrefs.GetInt(name + "builded", 0) == 1; 
    }

    public static void SetBuildProject(string name)
    {
        PlayerPrefs.SetInt(name + "builded", 1);
    }

    public static int GetLvl()
    {
        return PlayerPrefs.GetInt("Lvl", 6000);
    }

    public static void SetLvl(int lvl)
    {
        PlayerPrefs.SetInt("Lvl", lvl);
        OnLvlChenged?.Invoke(lvl);
    }

    public static void SetBuildingSaves(List<BuildingSave> save)
    {
        string data = JsonConvert.SerializeObject(save);

        PlayerPrefs.SetString("BuildingData", data);
    }

    public static List<BuildingSave> GetBuildingSaves()
    {
        string data = PlayerPrefs.GetString("BuildingData", "");
        return JsonConvert.DeserializeObject<List<BuildingSave>>(data);
    }
}

[Serializable]
public class BuildingSave
{
    public Vector3Int Position;
    public string Name;
    public int Money;
}
