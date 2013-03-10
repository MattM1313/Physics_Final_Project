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
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace SpriteClasses
{
    public class SpriteWithAnimations : Sprite
    {
        public AnimationDictionary spriteAnimations = new AnimationDictionary();

        // animation version
        public SpriteWithAnimations(Texture2D textureImage, Vector2 position, Vector2 velocity, bool setOrigin,
            float rotationSpeed, float scale, SpriteEffects spriteEffect)
            : base(textureImage, position, velocity, setOrigin, rotationSpeed, scale, spriteEffect)
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
            if (Active)
            {
                //put the current animation into a variable to make it easier to see what's happening
                Animation currentAnimation = spriteAnimations.animationDictionary[spriteAnimations.CurrentAnimation];

                //update the animation
                currentAnimation.Update(gameTime);
                //load the current image
                TextureImage = currentAnimation.cellList[currentAnimation.CurrentCell];
            }
        }
    }
}
