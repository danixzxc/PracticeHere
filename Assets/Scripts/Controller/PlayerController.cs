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
        #region variables and consts
        private float _xAxisInput;
        private bool _isJump;
        private bool _isMoving;

        private float _speed = 150f;
        private float _climbSpeed = 6f;
        private float _jumpSpeed = 9f;
        private float _slideSpeed = 2f;
        private float _movingThreshHold = .1f;
        private float _jumpThreshHold = 1f;

        private bool _wallGrab = false;
        private bool _wallCollision = false;

        private float _fallMultiplier = 2f;
        private float _lowJumpMultiplier = 2f;
        private float _xVelocity;
        private Vector3 _leftScale = new Vector3(-1, 1, 1);
        private Vector3 _rightScale = new Vector3(1, 1, 1);

        private AnimStatePlayer _track;

        private ObjectView _playerView;
        private Animator _animator;
        private readonly ContactPooler _contactPooler;

        #endregion
        public PlayerController(ObjectView player, Animator animator)
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
            _contactPooler.Update();
            _xAxisInput = Input.GetAxis("Horizontal");
            _isJump = Input.GetAxis("Vertical") > 0;
            _isMoving = Mathf.Abs(_xAxisInput) > _movingThreshHold;
            _playerView.transform.rotation = new Quaternion(_playerView.transform.rotation.x, _playerView.transform.rotation.y, 0, _playerView.transform.rotation.w);
            if (_isMoving) MoveTowards();


            if (_contactPooler.IsGrounded)
            {

                _animator.SetBool("isRunning", _isMoving);

                PlayerStartAnimation(_isMoving ? AnimStatePlayer.Run : AnimStatePlayer.Idle);

                if (_isJump && Mathf.Abs(_playerView.Rigidbody2D.velocity.y) <= _jumpThreshHold)
                {
                    _playerView.Rigidbody2D.AddForce(Vector2.up * _jumpSpeed, ForceMode2D.Impulse);
                }
            }
            else
            {
                if ((_isJump && Mathf.Abs(_playerView.Rigidbody2D.velocity.y) >= _jumpThreshHold))
                {
                    PlayerStartAnimation(AnimStatePlayer.Jump);
                    _animator.SetTrigger("Jumped");

                }
            }
            if (Mathf.Abs(_playerView.Rigidbody2D.velocity.y) >= _jumpThreshHold && _track!=AnimStatePlayer.Jump) 
            {
                PlayerStartAnimation(AnimStatePlayer.Fall);
            }



            _wallCollision = (_contactPooler.HasLeftContact || _contactPooler.HasRightContact) && !_contactPooler.IsGrounded;
            _wallGrab = _wallCollision && Input.GetKey(KeyCode.LeftShift) ;

            if (_wallCollision)
            {
                WallSlide();
                PlayerStartAnimation(_wallGrab ? AnimStatePlayer.WallClimb : AnimStatePlayer.WallSlide);

            }
            

            if (_wallGrab)
                _playerView.Rigidbody2D.velocity = _playerView.Rigidbody2D.velocity.Change(y: _climbSpeed);

            BetterJump();

        }

        private void PlayerStartAnimation(AnimStatePlayer animStatePlayer)
        {
            //_animator.StartAnimation
            //          (_playerView.SpriteRenderer, animStatePlayer, true, _animationSpeed);
            //_track = animStatePlayer;
            //как начальный костыль мб использовать в контроллере анимаций track, он в коде уже реализован, а потом код переписать? хотя фигня
        }

        private void BetterJump()
        { 
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
        }
    }

}
