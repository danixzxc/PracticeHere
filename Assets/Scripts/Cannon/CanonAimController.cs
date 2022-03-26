using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanonAimController
{
    private Transform _muzzleTransform;
    private Transform _targetTransform;

    private Vector3 _targetDirection; //These values are needed to rotate 
    private float _angleToRotate;
    private Vector3 _axisToRotate;


    public CanonAimController(Transform muzzleTransform, Transform targetTransform)
    {
        _muzzleTransform = muzzleTransform;
        _targetTransform = targetTransform;
    }

    public void Update()
    {
        _targetDirection = _targetTransform.position - _muzzleTransform.position;
        _angleToRotate = Vector3.Angle(Vector3.down, _targetDirection);
        _axisToRotate = Vector3.Cross(Vector3.down, _targetDirection);
        _muzzleTransform.rotation = Quaternion.AngleAxis(_angleToRotate, _axisToRotate);

        _muzzleTransform.transform.rotation = new Quaternion(0, _muzzleTransform.transform.rotation.y, _muzzleTransform.transform.rotation.z, _muzzleTransform.transform.rotation.w);
    }
}