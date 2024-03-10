using System;
using UnityEngine;

[CreateAssetMenu(fileName = "ShopItems", menuName = "ShopItems")]
public class ShopItems : ScriptableObject
{
    public ShopItem[] shopItems;
    public BuildingObjects buildingObjects;
}

[Serializable]
public class ShopItem
{
    public Sprite ActiveSprite;
    public Sprite DisabledSprite;
    public LevelOpen[] LevelOpens;
    public string Name;
    public int Cost;
}

[Serializable]
public struct LevelOpen
{
    public int Level;
    public int Count;
}