using UnityEngine;

public class SceneData : Singletone<SceneData>
{
    public BoxCollider2D gameAreaBounds;
    public Vector3 minBounds;
    public Vector3 maxBounds;
    public float minCameraZoom;
    public float maxCameraZoom;
    public float zoomSpeed;

    public override void Awake()
    {
        base.Awake();

        gameAreaBounds = Background.Instance.bounds;

        minBounds = gameAreaBounds.bounds.min;
        maxBounds = gameAreaBounds.bounds.max;
    }

    private void Update()
    {
        minBounds = gameAreaBounds.bounds.min;
        maxBounds = gameAreaBounds.bounds.max;
    }
}
