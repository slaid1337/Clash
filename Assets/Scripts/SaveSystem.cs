using System;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using UnityEngine.Rendering;

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
        return PlayerPrefs.GetInt("Money", 6000);
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
                foreach (var position in item.Positions)
                {
                    positions.Add(position);
                }
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
        return PlayerPrefs.GetInt("Lvl", 7000);
    }

    public static void AddLvl(int lvl)
    {
        int newLvl = GetLvl() + lvl;
        PlayerPrefs.SetInt("Lvl", newLvl);
        OnLvlChenged?.Invoke(newLvl);
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

    public static int GetQuestStage()
    {
        return PlayerPrefs.GetInt("QuestStage", 0);
    }

    public static void SetQuestStage(int stage)
    {
        PlayerPrefs.SetInt("QuestStage", stage);
    }

    public static bool IsQuestComplited(Quest quest)
    {
        string data = quest.BuildingName + quest.Exp + quest.Type;

        return PlayerPrefs.GetInt("IsComlite" + data, 0) == 1;
    }

    public static void SetQuestComplited(Quest quest)
    {
        string data = quest.BuildingName + quest.Exp + quest.Type;

        PlayerPrefs.SetInt("IsComlite" + data, 1);
    }
}

[Serializable]
public class BuildingSave
{
    public Vector3Int[] Positions;
    public Vector3Int SpawnPosition;
    public string Name;
    public int Money;
}
