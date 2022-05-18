using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;


namespace Configs
{
    public enum AnimStatePlayer //это реализовывается по-другому. столько кода не надо
    {
        Idle = 0,
        Run = 1,
        Jump = 2,
        Fall = 5,
        Somersault = 3,
        Attack = 4,
        WallSlide = 7,
        WallClimb = 8
    }


    [CreateAssetMenu(fileName = "SpriteAnimatorConfig", menuName = "Config/ Animator CFG", order = 0)]
    public class SpriteAnimatorConfig : ScriptableObject
    {
        public List<SpriteSequence> Sequences = new List<SpriteSequence>();

        [Serializable]
        public sealed class SpriteSequence
        {
            public AnimStatePlayer Track;
            public List<Sprite> Sprites = new List<Sprite>();
            public Animator Animator; //  мне нужен будет только аниматор, всё вроде в аниматоре лежит
        }
    }
}