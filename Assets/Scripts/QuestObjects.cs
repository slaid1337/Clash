using System;
using UnityEngine;

[CreateAssetMenu(fileName = "QuestObjects", menuName = "Object/QuestObjects")]
public class QuestObjects : ScriptableObject
{
    public Quest[] quests;
}

[Serializable]
public class Quest
{
    public QuestType Type;
    public string BuildingName;
    public int Exp;
}

public enum QuestType
{
    Building
}