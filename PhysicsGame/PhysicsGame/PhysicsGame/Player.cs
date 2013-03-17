using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpriteClasses;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace PhysicsGame
{
    public class Player : SpriteFromSheet
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
        public Player(
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
            int health,
            int strength,
            float defense
        )
            : base(textureImage, position, velocity, setOrigin, rotationSpeed, scale, spriteEffect, frameSize, currentFrame, sheetSize, totalTime)
        {
            Health = health;
            Strength = strength;
            Defense = defense;
        }
    }
}

