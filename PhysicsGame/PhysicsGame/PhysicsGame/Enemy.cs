using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpriteClasses;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

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

        public Enemy(Texture2D textureImage, Vector2 position, Vector2 velocity, bool setOrigin, float rotationSpeed, float scale, SpriteEffects spriteEffect, Vector2 frameSize, Vector2 currentFrame, Vector2 sheetSize, float totalTime)
            : base(textureImage, position, velocity, setOrigin, rotationSpeed, scale, spriteEffect, frameSize, currentFrame, sheetSize, totalTime)
        {
        }
    }
}
