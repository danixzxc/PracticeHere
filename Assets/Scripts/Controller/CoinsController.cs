using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Configs;
using System;

public class CoinsController : IDisposable
{

    private ObjectView _playerView;
    private List<CoinView> _coinViews;


    public CoinsController(ObjectView player, List<CoinView> coinViews)
    {
        _playerView = player;
        _coinViews = coinViews;
        _playerView.OnObjectContact += OnLevelObjectContact; //Event Subscribe
    }

    public void OnLevelObjectContact(CoinView contactObjectView)
    {
        if (_coinViews.Contains(contactObjectView))
        {
            GameObject.Destroy(contactObjectView.gameObject);
        }
    }

    public void Dispose()
    {
        _playerView.OnObjectContact -= OnLevelObjectContact; //Event UnSubscribe
        _coinViews.Clear();
    }
}
