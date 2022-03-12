using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
public class ParallaxController : IDisposable
{
    private Transform _camera;
    private Transform _back;
    private Vector3 _backStartPosition;
    private Vector3 _cameraStartPosition;
    private const float _coef = 0.1f;
    public ParallaxController(Transform camera, Transform back)
    {
        _camera = camera;
        _back = back;
        _backStartPosition = _back.transform.position;
        _cameraStartPosition = _camera.transform.position;

    }

    public void Update()
    {
       // _back.position = _backStartPosition + (_camera.position - _cameraStartPosition) * _coef;
        _back.position = _backStartPosition + new Vector3((_camera.position.x - _cameraStartPosition.x) * _coef, 0, 0);
    }

    public void Dispose()
    {
        //_activeAnimation.Clear();
    }
}
