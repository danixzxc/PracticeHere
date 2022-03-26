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

        private float _speed = 150f;
        private float _climbSpeed = 6f;
        private float _animationSpeed = 4f;
        private float _jumpSpeed = 9f;
        private float _slideSpeed = 2f;
        private float _movingThreshHold = .1f;
        private float _jumpThreshHold = 1f;

        private bool _wallGrab = false;
        private bool _wallCollision = false;

        private float _earthGravitation = -9.8f;
        private float _fallMultiplier = 2f;
        private float _lowJumpMultiplier = 2f;
        private float _yVelocity;
        private float _xVelocity;
        private Vector3 _leftScale = new Vector3(-1, 1, 1);
        private Vector3 _rightScale = new Vector3(1, 1, 1);


        private ObjectView _playerView;
        private SpriteAnimatorController _animator;
        private readonly ContactPooler _contactPooler;

        public PlayerController(ObjectView player, SpriteAnimatorController animator)
        {
            _playerView = player;
            _animator = animator;
            _animator.StartAnimation(_playerView.SpriteRenderer, AnimStatePlayer.Idle, true, _animationSpeed);
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
            

            _contactPooler.Update();
            _animator.Update();
            _xAxisInput = Input.GetAxis("Horizontal");
            _isJump = Input.GetAxis("Vertical") > 0;
            _isMoving = Mathf.Abs(_xAxisInput) > _movingThreshHold;
            _playerView.transform.rotation = new Quaternion(_playerView.transform.rotation.x, _playerView.transform.rotation.y, 0, _playerView.transform.rotation.w);
            if (_isMoving) MoveTowards();


            if (_contactPooler.IsGrounded)
            {
                _animator.StartAnimation
                    (_playerView.SpriteRenderer, _isMoving ? AnimStatePlayer.Run : AnimStatePlayer.Idle, true, _animationSpeed);
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
                }
            }
            if ((_contactPooler.HasLeftContact && !_contactPooler.IsGrounded)  ||
                (_contactPooler.HasRightContact && !_contactPooler.IsGrounded))
                    {
                WallSlide();
            }

            _wallCollision = _contactPooler.HasLeftContact || _contactPooler.HasRightContact;
            _wallGrab = _wallCollision && Input.GetKey(KeyCode.LeftShift);

            if (_wallGrab)
            {
                _playerView.Rigidbody2D.velocity = _playerView.Rigidbody2D.velocity.Change(y: _climbSpeed);
                _animator.StartAnimation
                         (_playerView.SpriteRenderer, AnimStatePlayer.WallClimb, true, _animationSpeed);
                //логика как с прыжком. если игрик растет то анимаци€ залазани€. а то шифт то посто€нно нажат и аним не робит
            }
            if (_wallCollision && _isJump)
            {
                _playerView.Rigidbody2D.velocity = Vector2.Lerp(_playerView.Rigidbody2D.velocity,
                                                        (new Vector2(-_playerView.transform.localScale.x * _climbSpeed, // минус взгл€д, потому что у мен€ сейчас перс в стенку смотрит а должен разворачиватьс€
                                                        _playerView.Rigidbody2D.velocity.y)), .5f * Time.deltaTime);
                //вроде не робит

            }
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
            _animator.StartAnimation
                (_playerView.SpriteRenderer, AnimStatePlayer.WallSlide, true, _animationSpeed);
            //Debug.Log("wall colission");
        }
    }

}
