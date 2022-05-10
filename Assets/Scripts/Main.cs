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

    private SpriteAnimatorConfig _coinAnimatorConfig;

    private SpriteAnimatorController _playerAnimator;
    private ParallaxController _parallax;
    private PlayerController _playerController;
    private CameraController _cameraController;

    private SpriteAnimatorController _coinAnimator;

    private CanonAimController _canonAimController;
    private BulletEmitterController _bulletEmitterController; 
    
    private CoinsController _coinsController;



    private void Start()
    {
        //куча всего одинаково объ€вл€етс€ и вызываетс€ апдейт. разве такой должен быть мейн? можно ли это прокинуть в один метод или цикл в массиве, или в целом пофиг ваще
        _coinAnimatorConfig = Resources.Load<SpriteAnimatorConfig>("CoinAnimationConfig"); 
        if (_coinAnimatorConfig) _coinAnimator = new SpriteAnimatorController(_coinAnimatorConfig);
        _playerAnimatorConfig = Resources.Load<SpriteAnimatorConfig>("SpriteAnimatorConfig");//он и в сериалайзед прокинуть мышкой и через resources load
        if (_playerAnimatorConfig)
        {
            _playerAnimator = new SpriteAnimatorController(_playerAnimatorConfig);
            _parallax = new ParallaxController(_levelView.Camera, _levelView.Back, _levelView.MiddleGroundSprite);
            _playerAnimator.StartAnimation(_playerView.SpriteRenderer, AnimStatePlayer.Idle, true, _animationSpeed) ; 
        }
        _playerController = new PlayerController(_playerView, animator);
        _cameraController = new CameraController(_playerView.Transform, _levelView.Camera);

        _canonAimController = new CanonAimController(_canonView.MuzzleTransform, _playerView.Transform);
        _bulletEmitterController = new BulletEmitterController(_canonView.Bullets, _canonView.EmitterTransform);

        _coinsController = new CoinsController(_playerView, _coinAnimator, _coinViews);

    }
    private void Update()
    {
        //Debug.Log("Hello world!" + Time.realtimeSinceStartup);
        Debug.Log(Time.frameCount);
        _parallax.Update();
        _playerController.Update(); // TODO мне не весь контроллер переделывать. просто не из конфига брать анимацию а из юнити анимаций, но логика така€ же, просто булки мен€ть
        _cameraController.Update();
        _canonAimController.Update();
        _bulletEmitterController.Update();
        //_coinAnimator.Update(); нужно будет переписать контроллеры и аниматоры, все вьюшки по мвс чтобы кодом на сцену спавн был. но наверн анимаци€ монетки на префабе - хорошее решенее, без гиперинженеринга
    }
}
