using System.Collections;
using System.Collections.Generic;
using System;
using Configs;
using UnityEngine;
using PlatformerMVC.Configs;

public class Main : MonoBehaviour
{
    [SerializeField] private SpriteAnimatorConfig _playerAnimatorConfig;
    [SerializeField] private int _animationSpeed;
    [SerializeField] private ObjectView  _playerView;
    [SerializeField] private LevelView _levelView;
    [SerializeField] private CannonView _canonView;

    private SpriteAnimatorController _playerAnimator;
    private ParallaxController _parallax;
    private PlayerController _playerController;
    private CameraController _cameraController;


    private CanonAimController _canonAimController;
    private BulletEmitterController _bulletEmitterController;


    private void Start()
    {
        _playerAnimatorConfig = Resources.Load<SpriteAnimatorConfig>("SpriteAnimatorConfig");
        if (_playerAnimatorConfig)
        {
            _playerAnimator = new SpriteAnimatorController(_playerAnimatorConfig);
            _parallax = new ParallaxController(_levelView.Camera, _levelView.Back, _levelView.MiddleGroundSprite);
            _playerAnimator.StartAnimation(_playerView.SpriteRenderer, AnimStatePlayer.Idle, true, _animationSpeed) ; 
        }
        _playerController = new PlayerController(_playerView, _playerAnimator);
        _cameraController = new CameraController(_playerView.Transform, _levelView.Camera);

        _canonAimController = new CanonAimController(_canonView.MuzzleTransform, _playerView.Transform);
        _bulletEmitterController = new BulletEmitterController(_canonView.Bullets, _canonView.EmitterTransform);
    }
    private void Update()
    {
        _parallax.Update();
        _playerController.Update();
        _cameraController.Update();
        _canonAimController.Update();
        _bulletEmitterController.Update();
    }
}
