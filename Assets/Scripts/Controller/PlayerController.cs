using System;
using System.Collections;
using System.Collections.Generic;
using Configs;
using PlatformerMVC.Configs;
using UnityEngine;


namespace PlatformerMVC.Configs
{

    public class PlayerController
    {
        private float _xAxisInput;
        private bool _isJump;
        private bool _isMoving;
        private bool _isClimbing;

        private float _speed = 150f;
        private float _climbSpeed = 6f;
        private float _animationSpeed = 4f;
        private float _jumpSpeed = 9f;
        private float _slideSpeed = 2f;
        private float _movingThreshHold = .1f;
        private float _climbingThreshHold = .1f;
        private float _jumpThreshHold = 1f;

        private bool _wallGrab = false;
        private bool _wallCollision = false;

        private float _fallMultiplier = 2f;
        private float _lowJumpMultiplier = 2f;
        private float _yVelocity;
        private float _xVelocity;
        private Vector3 _leftScale = new Vector3(-1, 1, 1);
        private Vector3 _rightScale = new Vector3(1, 1, 1);

        private AnimStatePlayer _track;

        private ObjectView _playerView;
        private SpriteAnimatorController _animator;
        private readonly ContactPooler _contactPooler;

        public PlayerController(ObjectView player, SpriteAnimatorController animator)
        {
            _playerView = player;
            _animator = animator;
            _contactPooler = new ContactPooler(_playerView.Collider2D);
        }


        public void MoveTowards()
        {
            _xVelocity = Time.fixedDeltaTime * _speed * (_xAxisInput < 0 ? -1 : 1);
            _playerView.Rigidbody2D.velocity = _playerView.Rigidbody2D.velocity.Change(x: _xVelocity);
            _playerView.transform.localScale = (_xAxisInput < 0 ? _leftScale : _rightScale);

        }

        public void Update()
        {
            Debug.Log("Has left contact" + _contactPooler.HasLeftContact);
            Debug.Log("Has right contact" + _contactPooler.HasRightContact);
            Debug.Log("Has ground contact" + _contactPooler.IsGrounded);

            _contactPooler.Update();
            _animator.Update();
            _xAxisInput = Input.GetAxis("Horizontal");
            _isJump = Input.GetAxis("Vertical") > 0;
            _isMoving = Mathf.Abs(_xAxisInput) > _movingThreshHold;
            _playerView.transform.rotation = new Quaternion(_playerView.transform.rotation.x, _playerView.transform.rotation.y, 0, _playerView.transform.rotation.w);
            if (_isMoving) MoveTowards();


            if (_contactPooler.IsGrounded)
            {
                Debug.Log("Playing idle or running animation" );

                _animator.StartAnimation
                    (_playerView.SpriteRenderer, _isMoving ? AnimStatePlayer.Run : AnimStatePlayer.Idle, true, _animationSpeed);
                _track = AnimStatePlayer.Run;

                if (_isJump && Mathf.Abs(_playerView.Rigidbody2D.velocity.y) <= _jumpThreshHold)
                {
                    _playerView.Rigidbody2D.AddForce(Vector2.up * _jumpSpeed, ForceMode2D.Impulse);
                }
            }
            else
            {
                if ((_isJump && Mathf.Abs(_playerView.Rigidbody2D.velocity.y) >= _jumpThreshHold))
                {
                    _animator.StartAnimation
                        (_playerView.SpriteRenderer, AnimStatePlayer.Jump, true, _animationSpeed);
                    _track = AnimStatePlayer.Jump;
                }
            }
            if (Mathf.Abs(_playerView.Rigidbody2D.velocity.y) >= _jumpThreshHold && _track!=AnimStatePlayer.Jump) 
            {
                _animator.StartAnimation
                    (_playerView.SpriteRenderer, AnimStatePlayer.Fall, true, _animationSpeed);
                _track = AnimStatePlayer.Fall;

            }




            _wallCollision = _contactPooler.HasLeftContact || _contactPooler.HasRightContact;
            _wallGrab = _wallCollision && Input.GetKey(KeyCode.LeftShift) && !_contactPooler.IsGrounded;

            if (_wallCollision && !_contactPooler.IsGrounded)
            {//для решения нынешней ситуации где все через апдейт вижу 2 варика - корутины или проверять каждый раз track
                //или переписать всё под обычную анимацию. Не, лень)
                WallSlide();
                    _animator.StartAnimation
                  (_playerView.SpriteRenderer,_wallGrab ? AnimStatePlayer.WallClimb : AnimStatePlayer.WallSlide, true, _animationSpeed);

            }
            //а ещё контроллер всё еще переполнен информацией. но я сделал получше
            //тонкости залазания на стенку и тд буду потом фиксить, сейчас главное основное
           //!осталось это починить
            if (_wallCollision && _isJump)
                _playerView.Rigidbody2D.velocity = Vector2.Lerp(_playerView.Rigidbody2D.velocity,
                                                        (new Vector2(_playerView.transform.localScale.x * _climbSpeed, //перед трансформом был минус. и так и так нет
                                                        _playerView.Rigidbody2D.velocity.y)), .5f * Time.deltaTime);



            if (_wallGrab)
                _playerView.Rigidbody2D.velocity = _playerView.Rigidbody2D.velocity.Change(y: _climbSpeed);

            //Better jump
            if (_playerView.Rigidbody2D.velocity.y < 0)
            {
                _playerView.Rigidbody2D.velocity += Vector2.up * Physics2D.gravity.y * (_fallMultiplier - 1) * Time.deltaTime;
            }
            else if (_playerView.Rigidbody2D.velocity.y > 0 && !_isJump)
            {
                _playerView.Rigidbody2D.velocity += Vector2.up * Physics2D.gravity.y * (_lowJumpMultiplier - 1) * Time.deltaTime;

            }

        }

        public void WallSlide()
        {
            _playerView.Rigidbody2D.velocity = _playerView.Rigidbody2D.velocity.Change(y: -_slideSpeed);
            

            //Debug.Log("wall colission");
        }
    }

}
