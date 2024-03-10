using UnityEngine;

public class CameraMoveSystem : MonoBehaviour
{
    private Camera _camera;
    private bool drag;
    private Vector3 mouseorigin;
    private SceneData _sceneData;
    PlayerInputData input;
    private Vector3 _newPosition;
    private Vector3 _smoothVelocity = Vector3.zero;
    [SerializeField] private float _smoothTime = 0.2f;
    [SerializeField] private float _speed;

    private enum MoveType { Horizontal, Vertical, Both };

    [SerializeField] private MoveType _moveType = MoveType.Horizontal;

    private void Start()
    {
        _camera = GetComponent<Camera>();
        _sceneData = SceneData.Instance;
        input = PlayerInputData.Instance;
        ClampPosition();
    }

    private void Update()
    {
        if (!input.rightButtonDown)
        {
            drag = false;
            
            if (_smoothVelocity.magnitude > 0 )
            {
                _camera.transform.position = Vector3.SmoothDamp(transform.position, _newPosition, ref _smoothVelocity, 0.2f);
                ClampPosition();
            }

            return;
        }
        
        switch (_moveType)
        {
            case MoveType.Horizontal:
                HorizontalMove();
                break;
            case MoveType.Vertical:
                VerticalMove();
                break;
            case MoveType.Both:
                BothMove();
                break;
        }
    }

    private void VerticalMove()
    {
        Vector3 difference = input.mousePos * _speed - _camera.transform.position;

        if (!drag)
        {
            drag = true;
            mouseorigin = input.mousePos * _speed;
        }

        Vector3 move = mouseorigin - difference;
        _newPosition = new Vector3(_camera.transform.position.x, move.y, _camera.transform.position.z);
        _camera.transform.position = Vector3.SmoothDamp(transform.position, _newPosition, ref _smoothVelocity, _smoothTime);

        float halfHeight = _camera.orthographicSize;
        float halfWidth = halfHeight * Screen.width / Screen.height;

        ClampPosition();
    }

    private void HorizontalMove()
    { 
        Vector3 difference = input.mousePos * _speed - _camera.transform.position;

        if (!drag)
        {
            drag = true;
            mouseorigin = input.mousePos * _speed;
        }

        Vector3 move = mouseorigin - difference;
        _newPosition = new Vector3(move.x, _camera.transform.position.y, _camera.transform.position.z);
        _camera.transform.position = Vector3.SmoothDamp(transform.position, _newPosition, ref _smoothVelocity, _smoothTime);

        float halfHeight = _camera.orthographicSize;
        float halfWidth = halfHeight * Screen.width / Screen.height;

        ClampPosition();
    }

    private void BothMove()
    {
        Vector3 difference = input.mousePos * _speed - _camera.transform.position;

        if (!drag)
        {
            drag = true;
            mouseorigin = input.mousePos * _speed;
        }

        Vector3 move = mouseorigin - difference;
        _newPosition = new Vector3(move.x, move.y, _camera.transform.position.z);
        _camera.transform.position = Vector3.SmoothDamp(transform.position, _newPosition, ref _smoothVelocity, _smoothTime);

        float halfHeight = _camera.orthographicSize;
        float halfWidth = halfHeight * Screen.width / Screen.height;

        ClampPosition();
    }

    private void ClampPosition()
    {
        float halfHeight = _camera.orthographicSize;
        float halfWidth = halfHeight * Screen.width / Screen.height;

        float clampedX = Mathf.Clamp(_camera.transform.position.x, _sceneData.minBounds.x + halfWidth, _sceneData.maxBounds.x - halfWidth);
        float clampedY = Mathf.Clamp(_camera.transform.position.y, _sceneData.minBounds.y + halfHeight, _sceneData.maxBounds.y - halfHeight);
        _camera.transform.position = new Vector3(clampedX, clampedY, _camera.transform.position.z);
    }
}
