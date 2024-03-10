using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoomSystem : MonoBehaviour
{
    private Camera _camera;
    private SceneData _sceneData;
    PlayerInputData input;

    private void Start()
    {
        _camera = Camera.main;
        _sceneData = SceneData.Instance;
        input = PlayerInputData.Instance;
    }

    private void Update()
    {
        _camera.orthographicSize = Mathf.Clamp(_camera.orthographicSize + input.zoomInput, _sceneData.minCameraZoom, _sceneData.maxCameraZoom);

        float halfHeight = _camera.orthographicSize;
        float halfWidth = halfHeight * Screen.width / Screen.height;

        float clampedX = Mathf.Clamp(_camera.transform.position.x, _sceneData.minBounds.x + halfWidth, _sceneData.maxBounds.x - halfWidth);
        float clampedY = Mathf.Clamp(_camera.transform.position.y, _sceneData.minBounds.y + halfHeight, _sceneData.maxBounds.y - halfHeight);
        _camera.transform.position = new Vector3(clampedX, clampedY, _camera.transform.position.z);
    }
}
