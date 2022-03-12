using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Configs;


public class PlayerController : MonoBehaviour
{
    private const float _walkSpeed = 3f;
    private const float _animationsSpeed = 10f; //хардкод, как мы ее потом меняем из юньки?
    private const float _jumpStartSpeed = 8f;
    private const float _movingThresh = 0.1f;
    private const float _flyThresh = 0.5f;
    private const float _groundLevel = 0.1f;
    private const float _g = -10f;

    private Vector3 _leftScale = new Vector3(-1, 1, 1);
    private Vector3 _rightScale = new Vector3(1, 1, 1);

    private float _yVelocity;
    private bool _doJump;
    private float _xAxisInput;

    private PlayerView _playerView;
    private SpriteAnimatorController _spriteAnimator;

    public PlayerController(PlayerView playerView, SpriteAnimatorController spriteAnimator)
    {
        _playerView = playerView;
        _spriteAnimator = spriteAnimator;
    }

    public void Update()
    {
        _doJump = Input.GetAxis("Vertical") > 0;
        _xAxisInput = Input.GetAxis("Horizontal");
        var goSideWay = Mathf.Abs(_xAxisInput) > _movingThresh;

        if (IsGrounded())
        {
            //walking
            if (goSideWay) GoSideWay();
            _spriteAnimator.StartAnimation(_playerView.SpriteRenderer, goSideWay ? AnimStatePlayer.Run : AnimStatePlayer.Idle, true, _animationsSpeed);

            //start jump
            if (_doJump && _yVelocity == 0)
            {
                _yVelocity = _jumpStartSpeed;
            }
            //stop jump
            else if (_yVelocity < 0)
            {
                _yVelocity = 0;
                _playerView.Transform.position = _playerView.Transform.position.Change(y: _groundLevel);
            }
        }
        else
        {
            //flying
            if (goSideWay) GoSideWay();
            if (Mathf.Abs(_yVelocity) > _flyThresh)
            {
                _spriteAnimator.StartAnimation(_playerView.SpriteRenderer, AnimStatePlayer.Jump, true, _animationsSpeed);
            }
            _yVelocity += _g * Time.deltaTime;
            _playerView.Transform.position += Vector3.up * (Time.deltaTime * _yVelocity);
        }
    }
    private void GoSideWay()
    {
        _playerView.Transform.position += Vector3.right * (Time.deltaTime * _walkSpeed * (_xAxisInput < 0 ? -1 : 1));
        _playerView.Transform.localScale = (_xAxisInput < 0 ? _leftScale : _rightScale);
    }

    public bool IsGrounded()
    {
        return _playerView.Transform.position.y <= _groundLevel + float.Epsilon && _yVelocity <= 0;
    }
}
