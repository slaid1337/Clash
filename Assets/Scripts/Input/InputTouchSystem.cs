using UnityEngine;

public class InputTouchSystem : InputSystem
{
    private PlayerInputData _playerInputData;
    private StaticData _staticData;
    private int _previousTouchCount;
    private float _lastTouchDistance;

    private void Start()
    {
        if (_camera  == null)
        {
            _camera = Camera.main;
        }
        
        _playerInputData = PlayerInputData.Instance;
        _staticData = StaticData.Instance;
    }

    public override void Run()
    {
        if (Input.touchCount == 1)
        {
            print(1);
            if (_previousTouchCount == 0)
            {
                _playerInputData.leftButtonDown = true;
            }
            else
            {
                _playerInputData.leftButtonDown = false;
            }

            _previousTouchCount = 1;

            _playerInputData.isDrawing = true;
            _playerInputData.leftButtonUp = false;
            _playerInputData.rightButtonDownOnce = false;
            _playerInputData.rightButtonDown = false;
            //_playerInputData.pointerDownOnce = Input.GetTouch(0).phase == TouchPhase.Began;
            _playerInputData.zoomInput = 0;
            _playerInputData.mousePos = _camera.ScreenToWorldPoint(new Vector3(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y, 0));
        }
        else if (Input.touchCount == 2)
        {
            Touch firstTouch = Input.GetTouch(0);
            Touch secondTouch = Input.GetTouch(1);

            float distTouch = (firstTouch.position - secondTouch.position).magnitude;

            

            if (_previousTouchCount == 1)
            {
                _playerInputData.leftButtonUp = true;
                _lastTouchDistance = distTouch;
            }
            else
            {
                _playerInputData.leftButtonUp = false;
            }

            _playerInputData.leftButtonDown = false;
            _playerInputData.mousePos = _camera.ScreenToWorldPoint( new Vector3( Input.GetTouch(0).position.x, Input.GetTouch(0).position.y, 0));
            _playerInputData.rightButtonDownOnce = false;
            _playerInputData.rightButtonDown = true;
            _playerInputData.isDrawing = false;

            float difference = distTouch - _lastTouchDistance;

            _playerInputData.zoomInput = difference * _staticData.zoomSpeedPhone;

            _lastTouchDistance = distTouch;
        }
        else
        {
            if (_previousTouchCount == 1)
            {
                _playerInputData.leftButtonUp = true;
            }
            else
            {
                _playerInputData.leftButtonUp = false;
            }

            _playerInputData.isDrawing = false;
            _previousTouchCount = 0;
            _playerInputData.mousePos = Vector3.zero;
            _playerInputData.rightButtonDownOnce = false;
            _playerInputData.rightButtonDown = false;
            _playerInputData.leftButtonDown = false;
            _playerInputData.zoomInput = 0;
        }
    }
}