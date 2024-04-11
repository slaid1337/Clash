using System;
using UnityEngine;

[CreateAssetMenu(fileName = "BuildingObjects", menuName = "BuildingObjects")]
public class BuildingObjects : ScriptableObject
{
    public BuildingObject[] objects;
}