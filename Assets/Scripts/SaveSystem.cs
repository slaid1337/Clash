using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
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

    public static void SaveOccupedFields(List<Vector3Int> data)
    {
        PlayerPrefs.SetString("OccupedFields", ConvertFieldsToString(data));
    }

    public static List<Vector3Int> GetOccupedFields()
    {
        string fieldsData = PlayerPrefs.GetString("OccupedFields");
        fieldsData += "4,-1,0;";
        return ConvertStringToFields(fieldsData);
    }

    private static List<Vector3Int> ConvertStringToFields(string data)
    {
        if (data.Length < 4) return new List<Vector3Int>();
        
        string[] fields = data.Split(';', StringSplitOptions.RemoveEmptyEntries);
        
        List<Vector3Int> result = new List<Vector3Int>();

        foreach (var item in fields)
        {
            string[] container = item.Split(',');

            Vector3Int newField = new Vector3Int(Int32.Parse(container[0]),
                Int32.Parse(container[1]),
                Int32.Parse(container[2]));
            result.Add(newField);
        }

        return result;
    }

    private static string ConvertFieldsToString(List<Vector3Int> data)
    {
        string fields = string.Empty;

        foreach (var item in data)
        {
            fields += $"{item.x},{item.y},{item.z};";
        }

        return fields;
    }

    public static int GetLvl()
    {
        return PlayerPrefs.GetInt("Lvl", 0);
    }

    public static void SetLvl(int lvl)
    {
        PlayerPrefs.SetInt("Lvl", lvl);
        OnLvlChenged?.Invoke(lvl);
    }
}

[Serializable]
public class BuildingSave
{
    public Vector3Int Position;
    public string Name;
    public int Money;
}
