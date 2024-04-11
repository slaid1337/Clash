using UnityEngine;

public class PlayerInputComponent : MonoBehaviour
{
    private InputSystem _inputSystem;
    [SerializeField] private Camera _camera;
    [SerializeField] private GameSettings _gameSettings;
    [SerializeField] private CheckWebGLPlatform _checkWebGLPlatform;

    private void Awake()
    {
        if (_checkWebGLPlatform.CheckIfMobile())
        {
            _inputSystem = gameObject.AddComponent<InputTouchSystem>();
        }
        else
        {
            _inputSystem = gameObject.AddComponent<InputMouseSystem>();
        }

        _inputSystem.SetCamera(_camera);
    }

    private void Update()
    {
        _inputSystem.Run();
    }
}
