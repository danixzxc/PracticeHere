using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletView : MonoBehaviour
{
    [SerializeField]
    private TrailRenderer _trail;
    [SerializeField]
    private SpriteRenderer _spriteRenderer;


    public void SetVisible(bool visible) //TODO логика во вьюшке
    {
        if (_trail) _trail.enabled = visible;
        if (_trail) _trail.Clear();

        _spriteRenderer.enabled = visible;
    }
}