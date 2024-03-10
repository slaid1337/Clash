using UnityEngine;

public class InputMouseSystem : InputSystem
{
    private PlayerInputData _playerInputData;
    private StaticData _staticData;

    private void Start()
    {
        if (_camera == null)
        {
            _camera = Camera.main;
        }
        _playerInputData = PlayerInputData.Instance;
        _staticData = StaticData.Instance;
    }

    public override void Run()
    {
        _playerInputData.zoomInput = Input.GetAxis("Mouse ScrollWheel") * _staticData.zoomSpeedPC;
        _playerInputData.previousMousePos = _playerInputData.mousePos;
        _playerInputData.mousePos = _camera.ScreenToWorldPoint(Input.mousePosition);
        _playerInputData.rightButtonDown = Input.GetMouseButton(1);
        _playerInputData.rightButtonDownOnce = Input.GetMouseButtonDown(1);
        _playerInputData.leftButtonDown = Input.GetMouseButtonDown(0);
        _playerInputData.leftButtonUp = Input.GetMouseButtonUp(0);
        _playerInputData.isDrawing = Input.GetMouseButton(0);
        _playerInputData.MouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
    }
}
