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

namespace SpriteClasses
{
    public class SpriteWithAnimations : Sprite
    {
        public Animation SpriteAnimation = new Animation();

        // animation version
        public SpriteWithAnimations(Texture2D textureImage, Vector2 position, Vector2 velocity, bool setOrigin,
            float rotationSpeed, float scale, SpriteEffects spriteEffect, SoundEffect collisionCueName, int scoreValue)
            : base(textureImage, position, velocity, setOrigin, rotationSpeed, scale, spriteEffect, collisionCueName, scoreValue)
         {   }
        //keep it on the screen
        public override void Update(GameTime gameTime, GraphicsDevice Device)
        {
            //call private method to update animation
            UpdateAndLoadAnimation(gameTime);
            //update the position and rotation, and keep it on the screen
            base.Update(gameTime, Device);
        }

        //don't keep it on the screen
        public override void Update(GameTime gameTime)
        {
            //call private method to update animation
            UpdateAndLoadAnimation(gameTime);
            //update the position and rotation, and keep it on the screen
            base.Update(gameTime);
        }

        //updates animation and loads next image into TextureImage for drawing
        private void UpdateAndLoadAnimation(GameTime gameTime)
        {
            //update the animation
            SpriteAnimation.Update(gameTime);
            //load the current image
            TextureImage = SpriteAnimation.cellList[SpriteAnimation.CurrentCell];
        }
    }
}
