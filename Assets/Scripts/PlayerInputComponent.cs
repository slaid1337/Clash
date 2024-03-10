using UnityEngine;

public class PlayerInputComponent : MonoBehaviour
{
    private InputSystem _inputSystem;
    [SerializeField] private Camera _camera;
    [SerializeField] private GameSettings _gameSettings;
    private InputType _inputType;

    private void Awake()
    {
        _inputType = _gameSettings.InputType;

        switch (_inputType)
        {
            case InputType.PC:
                _inputSystem = gameObject.AddComponent<InputMouseSystem>();
                break;
            default:
                _inputSystem = gameObject.AddComponent<InputMouseSystem>();
                break;
        }

        _inputSystem.SetCamera(_camera);
    }

    private void Update()
    {
        _inputSystem.Run();
    }
}
