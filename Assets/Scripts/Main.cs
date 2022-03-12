using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Configs;

public class Main : MonoBehaviour
{
    [SerializeField]
    private SpriteAnimatorConfig _playerAnimatorConfig;

    [SerializeField]
    private int _animationSpeed;
    
    [SerializeField]
    private PlayerView  _playerView;

    [SerializeField]
    private LevelView _levelView;

    private SpriteAnimatorController _playerAnimator;

    private ParallaxController _parallax;

    private PlayerController _playerController;

    private CameraController _cameraController;


    private void Start()
    {
        _playerAnimatorConfig = Resources.Load<SpriteAnimatorConfig>("SpriteAnimatorConfig");
        if (_playerAnimatorConfig)
        {
            _playerAnimator = new SpriteAnimatorController(_playerAnimatorConfig);
            _parallax = new ParallaxController(_levelView.Camera, _levelView.Back);
            _playerAnimator.StartAnimation(_playerView.SpriteRenderer, AnimStatePlayer.Idle, true, _animationSpeed) ; 
        }
        _playerController = new PlayerController(_playerView, _playerAnimator);
        _cameraController = new CameraController(_playerView.Transform, _levelView.Camera);
    }
    private void Update()
    {
        _playerAnimator.Update();
        _parallax.Update();
        _playerController.Update();
        _cameraController.Update();
    }
}
