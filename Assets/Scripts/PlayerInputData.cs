using UnityEngine;

public class PlayerInputData : Singletone<PlayerInputData>
{
    public float zoomInput;
    public bool rightButtonDown;
    public bool rightButtonDownOnce;
    public bool rightButtonUp;
    public bool leftButtonDown;
    public bool leftButtonUp;
    public bool isDrawing;
    public Vector3 previousMousePos;
    public Vector3 mousePos;
    public Vector2 MouseDelta;
}
