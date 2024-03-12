using System.Collections.Generic;
using UnityEngine;

public class BuildingSpawner : Singletone<BuildingSpawner>
{
    [SerializeField] private BuildingObjects _buildingObjects;
    private List<Building> _buildings;

    private void Start()
    {
        List<BuildingSave> saves = SaveSystem.GetBuildingSaves();
        _buildings = new List<Building>();

        if (saves != null)
        {
            foreach (BuildingSave save in saves)
            {
                foreach (var item in _buildingObjects.objects)
                {
                    if (item.Name == save.Name)
                    {
                        Building building = Instantiate(item.Prefab).GetComponent<Building>();
                        _buildings.Add(building);
                        building.OnLoad(save);
                        continue;
                    }
                }
            }
        }
    }

    public void SaveBuildings()
    {
        List<BuildingSave> saves = new List<BuildingSave>();

        foreach (var item in _buildings)
        {
            BuildingSave newSave = new BuildingSave();
            newSave.Position = item.Position;
            newSave.Name = item.Name;
            newSave.Money = (int) item.Money;

            saves.Add(newSave);
        }

        SaveSystem.SetBuildingSaves(saves);
    }
}
