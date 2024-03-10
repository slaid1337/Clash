using UnityEngine;

public abstract class InputSystem : MonoBehaviour
{
    [SerializeField] protected Camera _camera;
    public abstract void Run();

    public virtual void SetCamera(Camera camera)
    {
        _camera = camera;
    }
}
