using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectView : MonoBehaviour
{
    public SpriteRenderer SpriteRenderer;
    public Transform Transform;
    public Rigidbody2D Rigidbody2D;
    public Collider2D Collider2D;
    
    public Action<CoinView> OnObjectContact;

    private void OnTriggerEnter2D(Collider2D col)
    {
        col.gameObject.TryGetComponent(out CoinView levelObject);
        OnObjectContact?.Invoke(levelObject);
    }

}
