using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using Configs;

public class SpriteAnimatorController : IDisposable
{
    private sealed class Animation
    {
        public AnimStatePlayer Track;
        public List<Sprite> Sprites;
        public bool Loop;
        public float Speed = 10f;
        public float Counter;
        public bool Sleep;

    public void Update()
    {
        if (Sleep) return;
        Counter += Time.deltaTime * Speed;
            if (Loop)
            {
                while (Counter > Sprites.Count)
                {
                    Counter -= Sprites.Count;
                }
            }
            else if (Counter > Sprites.Count)
            {  
                Counter = Sprites.Count;
                Sleep = true;   
            }
    }
}

public void Update()
{
    foreach(var animation in _activeAnimation)
    {
        animation.Value.Update();
        if (animation.Value.Counter < animation.Value.Sprites.Count)
        {
            animation.Key.sprite = animation.Value.Sprites[(int)animation.Value.Counter];
        }    

    }
}

private SpriteAnimatorConfig _config;
private Dictionary<SpriteRenderer, Animation> _activeAnimation =
        new Dictionary<SpriteRenderer, Animation>();

public SpriteAnimatorController(SpriteAnimatorConfig config)
{
    _config = config;
}

public void StartAnimation(SpriteRenderer spriteRenderer, AnimStatePlayer track, bool loop, float speed)
{
    if(_activeAnimation.TryGetValue(spriteRenderer, out var animation))
    {
        animation.Sleep = false;
        animation.Loop = loop;
        animation.Speed = speed;

        if(animation.Track != track)
        {
            animation.Track = track;
            animation.Sprites = _config.Sequences.Find(sequence => sequence.Track == track).Sprites;
            animation.Counter = 0;

        }
    }
    else 
    {
            _activeAnimation.Add(spriteRenderer, new Animation()
            {
                Loop = loop,
                Speed = speed,
                Track = track,
                Sprites = _config.Sequences.Find(sequence => sequence.Track == track).Sprites
            }) ;
        
    }
}
 
    public void StopAnimation(SpriteRenderer spriteRenderer)
{
    if(_activeAnimation.ContainsKey(spriteRenderer))
    {
        _activeAnimation.Remove(spriteRenderer);
    }
}

public void Dispose()
    {
        _activeAnimation.Clear();
    }
}
