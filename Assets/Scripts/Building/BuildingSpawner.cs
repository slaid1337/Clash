using System.Collections.Generic;
using UnityEngine;

public class BuildingSpawner : Singletone<BuildingSpawner>
{
    [SerializeField] private BuildingObjects _buildingObjects;
    private List<Building> _buildings;


    public override void Awake()
    {
        base.Awake();

        GameLoader.Instance.OnLoad.AddListener(OnLoad);
    }

    public void OnLoad()
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

    public Building SpawnNewBuild(BuildingObject buildingObject, Vector3Int[] positions, Vector3Int spawnedPosition)
    {
        Building building = Instantiate(buildingObject.Prefab).GetComponent<Building>();
        BuildingSave newSave = new BuildingSave();

        building.StartPosition = spawnedPosition;

        newSave.Positions = building.GetPositions().ToArray();
        newSave.SpawnPosition = spawnedPosition;
        newSave.Name = buildingObject.Name;
        newSave.Money = 0;
        _buildings.Add(building);
        building.OnLoad(newSave);
        GridBuildingSystem.Instance.AddOccupedField(building.GetPositions().ToArray());
        SaveBuildings();
        BuildingSystem.Instance.OnBuildNewBuilding?.Invoke(building.Name);
        return building;
    }

    public void AddBuilding(Building building, BuildingObject buildingObject, Vector3Int[] positions, Vector3Int spawnedPosition)
    {
        BuildingSave newSave = new BuildingSave();
        newSave.Positions = positions;
        newSave.SpawnPosition = spawnedPosition;
        newSave.Name = buildingObject.Name;
        newSave.Money = 0;
        _buildings.Add(building);
        building.OnLoad(newSave);
        GridBuildingSystem.Instance.AddOccupedField(positions);
        SaveBuildings();
    }

    public void SaveBuildings()
    {
        List<BuildingSave> saves = new List<BuildingSave>();

        foreach (var item in _buildings)
        {
            BuildingSave newSave = new BuildingSave();
            newSave.Positions = item.GetPositions().ToArray();
            newSave.Name = item.Name;
            newSave.Money = (int) item.Money;
            newSave.SpawnPosition = item.StartPosition;
            saves.Add(newSave);
        }

        SaveSystem.SetBuildingSaves(saves);
    }
}
