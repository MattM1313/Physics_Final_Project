using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpriteClasses;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace PhysicsGame
{
    public class Enemy : SpriteFromSheet
    {
        protected int health;
        public int Health
        {
            get { return health; }
            set { health = value; }
        }
        protected int strength;
        public int Strength
        {
            get { return strength; }
            set { strength = value; }
        }
        protected float defense;
        public float Defense
        {
            get { return defense; }
            set { defense = value; }
        }
        protected bool hasHit;
        public bool HasHit
        {
            get { return hasHit; }
            set { hasHit = value; }
        }


        public Enemy(
            Texture2D textureImage,
            Vector2 position,
            Vector2 velocity,
            bool setOrigin,
            float rotationSpeed,
            float scale,
            SpriteEffects spriteEffect,
            Vector2 frameSize,
            Vector2 currentFrame,
            Vector2 sheetSize,
            float totalTime,  
            SoundEffect collisionCueName,
            int scoreValue,
            int health,
            int strength,
            float defense,
            bool hasHit
        )
            : base(textureImage, position, velocity, setOrigin, rotationSpeed, scale, spriteEffect, frameSize, currentFrame, sheetSize, totalTime, collisionCueName, scoreValue )
        {
            Health = health;
            Strength = strength;
            Defense = defense;
            HasHit = hasHit;

        }
    }
}
