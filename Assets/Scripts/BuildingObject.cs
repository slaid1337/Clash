using System;
using UnityEngine;

[CreateAssetMenu(fileName = "BuildingObject", menuName = "BuildingObject")]
public class BuildingObjects : ScriptableObject
{
    public BuildingObject[] objects;
}

[Serializable]
public class BuildingObject
{
    public string Name;
    public int Income;
    public int Capacity;
    public GameObject Prefab;
}