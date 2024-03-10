using UnityEngine;

[CreateAssetMenu(fileName = "GameSettings", menuName = "Object/GameSettings")]
public class GameSettings : ScriptableObject
{
    public InputType InputType;
}

public enum InputType
{
    Mobile,
    PC
}