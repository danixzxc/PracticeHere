using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletEmitterController
{
    private List<BulletController> _bullets = new List<BulletController>();
    private Transform _transform;

    private int _currentIndex;
    private float _timeBetweenBulletShots;
    private const float _delay = 1f;
    private const float _startingSpeed = 9;



    public BulletEmitterController(List<ObjectView> bulletViews, Transform transform)
    {
        _transform = transform;
        foreach (var bulletView in bulletViews)
        {
            _bullets.Add(new BulletController(bulletView));
        }
    }

    public void Update()
    {
        _transform.rotation = new Quaternion(0, _transform.transform.rotation.y, _transform.transform.rotation.z, _transform.transform.rotation.w);

        if (_timeBetweenBulletShots > 0)
        {
            _bullets[_currentIndex].Active(false);
            _timeBetweenBulletShots -= Time.deltaTime;
        }
        else
        {
            _timeBetweenBulletShots = _delay;
            _bullets[_currentIndex].Shoot(_transform.position, _transform.right * _startingSpeed);
            _currentIndex++;
            if (_currentIndex >= _bullets.Count)
            {
                _currentIndex = 0;
            }
        }
    }
}