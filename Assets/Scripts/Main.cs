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

    [SerializeField] private Animator animator;


    [SerializeField] private List<CoinView> _coinViews;
    private ParallaxController _parallax;
    private PlayerController _playerController;
    private CameraController _cameraController;

    private SpriteAnimatorController _coinAnimator;
    private CoinsController _coinsController;
    private CanonAimController _canonAimController;
    private BulletEmitterController _bulletEmitterController; 
    



    private void Start()
    {
        //куча всего одинаково объ€вл€етс€ и вызываетс€ апдейт. разве такой должен быть мейн? можно ли это прокинуть в один метод или цикл в массиве, или в целом пофиг 
         _coinsController = new CoinsController(_playerView, _coinViews);
             _parallax = new ParallaxController(_levelView.Camera, _levelView.Back, _levelView.MiddleGroundSprite);
        _playerController = new PlayerController(_playerView, animator);
        _cameraController = new CameraController(_playerView.Transform, _levelView.Camera);
        Debug.Log("lowbars are good, but you need to strive to high bars!");
        _canonAimController = new CanonAimController(_canonView.MuzzleTransform, _playerView.Transform);
        _bulletEmitterController = new BulletEmitterController(_canonView.Bullets, _canonView.EmitterTransform);


    }
    private void Update()
    {
        _parallax.Update();
        _playerController.Update(); // TODO мне не весь контроллер переделывать. просто не из конфига брать анимацию а из юнити анимаций, но логика така€ же, просто булки мен€ть
        _cameraController.Update();
        _canonAimController.Update();
        _bulletEmitterController.Update();
        }
}
