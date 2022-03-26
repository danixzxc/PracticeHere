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
    private const float _coef = 0.3f;
    private Sprite _sprite;
    private float textureSize;
    public ParallaxController(Transform camera, Transform back, Sprite sprite)
    {
        _sprite = sprite;
        _camera = camera;
        _back = back;
        _backStartPosition = _back.transform.position;
        _cameraStartPosition = _camera.transform.position;
    }

    public void Update()
    {
        _back.position = new Vector3(_backStartPosition.x + (_camera.position.x - _cameraStartPosition.x) * _coef, _backStartPosition.y, _backStartPosition.z);
        textureSize = _sprite.texture.width / _sprite.pixelsPerUnit;
        //_cameraStartPosition = _camera.position;
        if (Mathf.Abs(_camera.position.x - _back.position.x) >= textureSize)
        {
            float offsetPosition = (_cameraStartPosition.x - _back.position.x) % textureSize;
            _backStartPosition = new Vector3(_camera.position.x + offsetPosition, _backStartPosition.y);
        }
    }

    public void Dispose()
    {
        //_activeAnimation.Clear();
    }
}
