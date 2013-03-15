using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpriteClasses;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace PhysicsGame
{
    public class Barrier : Sprite
    {
        protected int health;
        public int Health
        {
            get { return health; }
            set { health = value; }
        }
        public Barrier(
            Texture2D textureImage,
            Vector2 position,
            Vector2 velocity,
            bool setOrigin,
            float rotationSpeed,
            float scale,
            SpriteEffects spriteEffect,
            float totalTime,
            int health
        )
            : base(textureImage, position, velocity, setOrigin, rotationSpeed, scale, spriteEffect)
        {
            Health = health;
        }

    }
}
