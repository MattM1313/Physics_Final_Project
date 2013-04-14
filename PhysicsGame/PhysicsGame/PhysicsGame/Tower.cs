using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using SpriteClasses;

namespace PhysicsGame
{
    public class Tower : Sprite
    {
        protected int health;
        public int Health
        {
            get { return health; }
            set { health = value; }
        }

        public Tower(Texture2D textureImage, Vector2 position,
            float scale, SpriteEffects spriteEffect, int health)
            : base(textureImage, position, new Vector2(0, 0), true, 0, scale, SpriteEffects.None, null, 0)
        {

            Health = health;

        }

    


    }
}
