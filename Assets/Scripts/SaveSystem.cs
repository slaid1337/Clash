using System;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Cysharp.Threading.Tasks;
using Eiko.YaSDK.Data;

public static class SaveSystem
{
    // эвент на изменение денег
    public static Action<int> OnMoneyChenged;
    // эвент на изменение уровня
    public static Action<int> OnLvlChenged;

    public static  void SpendMoney(int cost, bool isCloud = false)
    {
        SetMoney(  GetMoney() - cost, isCloud);
    }

    /// <summary>
    /// устанавливает значение денег
    /// </summary>
    /// <param name="money">деньги</param>
    public static void SetMoney(int money, bool isCloud = false)
    {
        SetIntValue("Money", money, isCloud);
        OnMoneyChenged?.Invoke(money);
    }

    /// <summary>
    /// добавляет деньги
    /// </summary>
    /// <param name="money">деньги</param>
    public static void AddMoney(int money)
    {
        int newMoney = money +  GetMoney();
        SetIntValue("Money", newMoney);
        OnMoneyChenged?.Invoke(newMoney);
    }

    /// <summary>
    /// получает текущее значение денег
    /// </summary>
    /// <returns>деньги</returns>
    public static int GetMoney(bool isCloud = false)
    {
        return GetIntValue("Money", 100);
    }

    /// <summary>
    /// проверка на первый запуск
    /// </summary>
    /// <returns></returns>
    public static bool IsFirstStart()
    {
        return GetIntValue("IsFirstStart", 0) == 0 ? true : false;
    }

    /// <summary>
    /// устанавливает что запуск не первый
    /// </summary>
    public static void SetFirstStart()
    {
        SetIntValue("IsFirstStart", 1);
    }

    public static List<Vector3Int> GetOccupedFields(bool isCloud = false)
    {
        List<BuildingSave> saves =GetBuildingSaves();
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

    public static bool CheckOnBuidProject(string name, bool isCloud = false)
    {
        return  GetIntValue(name + "builded", 0) == 1; 
    }

    public static void SetBuildProject(string name, bool isCloud = false)
    {
        SetIntValue(name + "builded", 1);
    }

    public static void SetBuildiProjectsNames(List<string> names, bool isCloud = false)
    {
        string data = JsonConvert.SerializeObject(names);

        SetStringValue("BuildProjectNames", data, isCloud);
    }

    public static List<string> GetBuildiProjectsNames(bool isCloud = false)
    {
        List<string> names = new List<string>();

        string data = GetStringValue("BuildProjectNames", "");

        if (data.Length == 0)
        {
            return names;
        }

        names = JsonConvert.DeserializeObject<List<string>>(data);

        return names;
    }

    public static int GetLvl(bool isCloud = false)
    {
        return  GetIntValue("Lvl", 0, isCloud);
    }

    public static  void AddLvl(int lvl)
    {
        int newLvl = GetLvl() + lvl;
        SetIntValue("Lvl", newLvl);
        OnLvlChenged?.Invoke(newLvl);
    }

    public static void SetLvl(int lvl, bool isCloud = false)
    {
        SetIntValue("Lvl", lvl, isCloud);
        OnLvlChenged?.Invoke(lvl);
    }

    public static void SetBuildingSaves(List<BuildingSave> save, bool isCloud = false)
    {
        string data = JsonConvert.SerializeObject(save);

        SetStringValue("BuildingData", data, isCloud);
    }

    public static List<BuildingSave> GetBuildingSaves(bool isCloud = false)
    {
        string data = GetStringValue("BuildingData", "");
        return JsonConvert.DeserializeObject<List<BuildingSave>>(data);
    }

    public static int GetQuestStage(bool isCloud = false)
    {
        return  GetIntValue("QuestStage", 0);
    }

    public static void SetQuestStage(int stage, bool isCloud = false)
    {
        SetIntValue("QuestStage", stage, isCloud);
    }

    public static bool IsQuestComplited(Quest quest, bool isCloud = false)
    {
        string data = quest.BuildingName + quest.Exp + quest.Type;

        return GetIntValue("IsComlite" + data, 0) == 1;
    }

    public static void SetQuestComplited(Quest quest, bool isCloud = false)
    {
        string data = quest.BuildingName + quest.Exp + quest.Type;

        SetIntValue("IsComlite" + data, 1, isCloud);
    }

    public static void SetQuestNotComplited(Quest quest, bool isCloud = false)
    {
        string data = quest.BuildingName + quest.Exp + quest.Type;

        SetIntValue("IsComlite" + data, 0, isCloud);
    }

    private static int GetIntValue(string key, int defaultValue, bool isCloud = false)
    {

        return YandexPrefs.GetInt(key, defaultValue);

    }

    private static string GetStringValue(string key, string defaultValue, bool isCloud = false)
    {

        return YandexPrefs.GetString(key, defaultValue);

    }

    private static void SetIntValue(string key, int value, bool isCloud = false)
    {
        YandexPrefs.SetInt(key, value);
    }

    private static void SetStringValue(string key, string value, bool isCloud = false)
    {
        YandexPrefs.SetString(key, value);
    }

    public static void SetSaveDate(string date, bool isCloud = false)
    {
        SetStringValue("date", date, isCloud);
    }

    public static string GetSaveDate(bool isCloud = false)
    {
        return GetStringValue("date", "", isCloud);
    }

    public static void SetLang(string lang)
    {
        SetStringValue("Language", lang);
    }

    public static string GetLang()
    {
        return GetStringValue("Language", "");
    }

    public static async UniTask<bool> LoadCloudData()
    {
        int money =  GetMoney(isCloud: true);
        int lvl =  GetLvl(isCloud: true);
        QuestObjects quests = QuestPanel.Instance.QuestObjects;
        int questStage =  GetQuestStage(isCloud: true);
        List<BuildingSave> BuildingsData = GetBuildingSaves(isCloud: true);
        List<string> buildProjectsNames = GetBuildiProjectsNames(isCloud: true);

        SetMoney(money);
        SetLvl(lvl);
        SetQuestStage(questStage);

        foreach (var item in quests.quests)
        {
            if ( IsQuestComplited(item, isCloud: true))
            {
                SetQuestComplited(item);
            }
        }

        SetBuildingSaves(BuildingsData);
        SetBuildiProjectsNames(buildProjectsNames);

        return true;
    }

    public static bool SendCloudData()
    {
        int money = GetMoney();
        int lvl =  GetLvl();
        QuestObjects quests = QuestPanel.Instance.QuestObjects;
        int questStage =  GetQuestStage();
        List<BuildingSave> BuildingsData = GetBuildingSaves();
        List<string> buildProjectsNames = GetBuildiProjectsNames();

        SetMoney(money, isCloud:true);
        SetLvl(lvl, isCloud: true);
        SetQuestStage(questStage, isCloud: true);

        foreach (var item in quests.quests)
        {
            if ( IsQuestComplited(item))
            {
                SetQuestComplited(item, isCloud: true);
            }
        }

        SetBuildingSaves(BuildingsData, isCloud: true);
        SetBuildiProjectsNames(buildProjectsNames, isCloud: true);

        return true;
    }

    public static void test()
    {
        SetStringValue("test", "12345", isCloud: true);
        YandexPrefs.SetString("testPrefs", "2222222");
    }

    public static void DeleteAllSaves()
    {
        List<BuildingSave> BuildingsData = new List<BuildingSave>();
        List<string> buildProjectsNames = new List<string>();

        QuestObjects quests = QuestPanel.Instance.QuestObjects;


        SetBuildingSaves(BuildingsData, isCloud: true);
        SetBuildiProjectsNames(buildProjectsNames, isCloud: true);

        SetMoney(100);
        SetLvl(0);
        SetQuestStage(0);

        foreach (var item in quests.quests)
        {
            SetQuestNotComplited(item);
        }

        PlayerPrefs.DeleteAll();
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
