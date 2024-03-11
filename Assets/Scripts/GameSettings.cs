using UnityEngine;

[CreateAssetMenu(fileName = "GameSettings", menuName = "Object/GameSettings")]
public class GameSettings : ScriptableObject
{
    public InputType InputType;

    public Vector3Int[] ClampedPositions;
}

public enum InputType
{
    Mobile,
    PC
}