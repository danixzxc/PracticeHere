using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController
{

    private float x;
    private float y;

    private float offSetX = 1.5f;
    private float offSetY = 1.5f;

    private int _camSpeed = 150;
    private Transform _playerTransform;
    private Transform _cameraTransform;


    public CameraController(Transform player, Transform camera)
    {
        _playerTransform = player;
        _cameraTransform = camera;
    }

    public void Update()
    {
        x = _playerTransform.position.x;
        y = _playerTransform.position.y;

        _cameraTransform.position =
            Vector3.Lerp(_cameraTransform.position, new Vector3(x + offSetX, y + offSetY, _cameraTransform.position.z),
                Time.deltaTime * _camSpeed);
    }


}