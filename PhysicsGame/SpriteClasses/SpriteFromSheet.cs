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
    public class SpriteFromSheet : Sprite
    {
        protected Vector2 frameSize;      // Size of frames in sprite sheet
        public Vector2 FrameSize
        {
            get { return frameSize; }
            set { frameSize = value; }
        }
        protected Vector2 currentFrame;   // Index of current frame in sprite sheet
        public Vector2 CurrentFrame
        {
            get { return currentFrame; }
            set { currentFrame = value; }
        }
        protected Vector2 sheetSize;      // Number of columns and rows in the sprite sheet
        public Vector2 SheetSize
        {
            get { return sheetSize; }
            set { sheetSize = value; }
        }
        // Time when we need to goto next frame
        private float TimeToNextFrame { get; set; }
        // time so far towards next frame
        private float ElapsedTime { get; set; }
        // total Time for whole sheet to play
        public float TotalTime { get; set; }

        //rectangle occupied by texture
        public override Rectangle CollisionRectangle
        {
            get
            {
                return new Rectangle((int)(position.X - SpriteOrigin.X * Scale), (int)(position.Y - SpriteOrigin.Y * Scale), (int)(FrameSize.X * Scale), (int)(FrameSize.Y * Scale));
            }
        }

        //sprite sheet version
        public SpriteFromSheet(Texture2D textureImage, Vector2 position, Vector2 velocity,
            bool setOrigin, float rotationSpeed, float scale, SpriteEffects spriteEffect,
            Vector2 frameSize, Vector2 currentFrame, Vector2 sheetSize, float totalTime, SoundEffect collisionCueName, int scoreValue)
            : base(textureImage, position, velocity, setOrigin, rotationSpeed, scale, spriteEffect, collisionCueName, scoreValue)
        {
            FrameSize = frameSize;
            CurrentFrame = currentFrame;
            SheetSize = sheetSize;
            //time to next frame is total time to play the sheet divided by number of frames
            TotalTime = totalTime;
            TimeToNextFrame = TotalTime / (sheetSize.X * sheetSize.Y);
            if (SetOrigin)
            {
                SpriteOrigin = new Vector2(FrameSize.X / 2, FrameSize.Y / 2);
            }
        }        

        //loops through entire spritesheet, called by autosprites in dodgeblade so they
        //can use polymorphism and check against player position
        public virtual void Update(GameTime gameTime, Vector2 player)
        {
            Update(gameTime);
        }

        //loops through entire spritesheet, called by autosprites in dodgeblade so they
        //can use polymorphism and check against player position
        //--this version keeps sprite on screen
        public virtual void Update(GameTime gameTime, GraphicsDevice Device, Vector2 player)
        {
            Update(gameTime, Device);
        }

        //loops through entire spritesheet
        public override void Update(GameTime gameTime)
        {
            if (Active)
            {
                //call base method to do rotation and basic movement
                base.Update(gameTime);
                //how long since last frame displayed
                ElapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
                //if it's time to display the next one, do so, then reset time back to 0
                if (ElapsedTime > TimeToNextFrame)
                {
                    if (currentFrame.X >= sheetSize.X - 1)
                    {
                        currentFrame.X = 0;
                        if (currentFrame.Y >= sheetSize.Y - 1)
                        {
                            currentFrame.Y = 0;
                        }
                        else
                        {
                            currentFrame.Y++;
                        }
                    }
                    else
                    {
                        currentFrame.X++;
                    }
                    ElapsedTime = 0f;
                }
            }
        }

        //loops through entire spritesheet
        //--this version keeps sprite on screen
        public override void Update(GameTime gameTime, GraphicsDevice Device)
        {
            if (Active)
            {
                //call overload to do rotation, basic movement and frame updating
                Update(gameTime);
                //keep on screen
                if (Position.X > Device.Viewport.Width - (FrameSize.X - SpriteOrigin.X) * Scale)
                {
                    position.X = Device.Viewport.Width - (FrameSize.X - SpriteOrigin.X) * Scale;
                    velocity.X = -Velocity.X;
                }
                else if (Position.X < SpriteOrigin.X)
                {
                    position.X = SpriteOrigin.X;
                    velocity.X = -Velocity.X;
                }

                if (Position.Y > Device.Viewport.Height - (FrameSize.Y - SpriteOrigin.Y) * Scale)
                {
                    position.Y = Device.Viewport.Height - (FrameSize.Y - SpriteOrigin.Y) * Scale;
                    velocity.Y = -Velocity.Y;
                }
                else if (Position.Y < SpriteOrigin.Y)
                {
                    position.Y = SpriteOrigin.Y;
                    velocity.Y = -Velocity.Y;
                }
            }
        }

        //loops through one row of spritesheet (first row is row 0)
        public virtual void Update(GameTime gameTime, GraphicsDevice Device, int row)
        {
            if (Active)
            {
                base.Update(gameTime, Device);
                //how long since last frame displayed
                ElapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
                //if it's time to display the next one, do so, then reset time back to 0
                if (ElapsedTime > TimeToNextFrame)
                {
                    currentFrame.Y = row;

                    if (currentFrame.X >= sheetSize.X - 1)
                    {
                        currentFrame.X = 0;
                    }
                    else
                    {
                        currentFrame.X++;
                    }
                    ElapsedTime = 0f;
                }
            }
        }

        //loops through one row of spritesheet (first row is row 0)
        //--this version keeps sprite on screen
        public virtual void Update(GameTime gameTime, int row)
        {
            if (Active)
            {
                base.Update(gameTime);
                //how long since last frame displayed
                ElapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
                //if it's time to display the next one, do so, then reset time back to 0
                if (ElapsedTime > TimeToNextFrame)
                {
                    currentFrame.Y = row;

                    if (currentFrame.X >= sheetSize.X - 1)
                    {
                        currentFrame.X = 0;
                    }
                    else
                    {
                        currentFrame.X++;
                    }
                    ElapsedTime = 0f;
                }
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (Active)
            {
                spriteBatch.Draw(TextureImage,
                                 Position,
                                 new Rectangle((int)(currentFrame.X * frameSize.X),
                                               (int)(currentFrame.Y * frameSize.Y),
                                               (int)frameSize.X,
                                               (int)frameSize.Y),
                                 Color.White,
                                 Rotation,
                                 SpriteOrigin,
                                 Scale,
                                 SpriteEffect,
                                 0);
            }
        }

        //determine if sprite has gone off screen
        public override bool IsOffScreen(GraphicsDevice Device)
        {
            if (position.X < -((FrameSize.X - SpriteOrigin.X) * Scale) ||
                position.X > Device.Viewport.Width + (FrameSize.X - SpriteOrigin.X) * Scale ||
                position.Y < -((FrameSize.Y - SpriteOrigin.Y) * Scale) ||
                position.Y > Device.Viewport.Height + (FrameSize.Y - SpriteOrigin.Y) * Scale)
            {
                return true;
            }
            return false;
        }

        //is there a collision with another sprite?
        public override bool CollisionSprite(Sprite otherSprite)
        {
            //cast the other sprite as a SpriteFromSheet - this method
            //is coded to work with two spriteFromSheet objects only
            SpriteFromSheet sprite = (SpriteFromSheet)otherSprite;
            bool hit = false;

            // Update the passed object's transform
            // SEQUENCE MATTERS HERE - DO NOT REARRANGE THE ORDER OF THE TRANSFORMATIONS BELOW
            Matrix spriteTransform =
                Matrix.CreateTranslation(new Vector3(-sprite.SpriteOrigin, 0.0f)) *
                Matrix.CreateScale(sprite.Scale) *  //would go here
                Matrix.CreateRotationZ(sprite.Rotation) *
                Matrix.CreateTranslation(new Vector3(sprite.Position, 0.0f));

            // Build the calling object's transform
            // SEQUENCE MATTERS HERE - DO NOT REARRANGE THE ORDER OF THE TRANSFORMATIONS BELOW
            Matrix thisTransform =
                Matrix.CreateTranslation(new Vector3(-SpriteOrigin, 0.0f)) *
                Matrix.CreateScale(Scale) *
                Matrix.CreateRotationZ(Rotation) *
                Matrix.CreateTranslation(new Vector3(Position, 0.0f));

            // Calculate the bounding rectangle of the passed object in world space
            //For the bounding rectangle, can't use CollisionRectangle property because
            //it adjusts for origin and scale, transform does both of those things for us, so 
            //we just need a simple bounding rectangle here

            //With transformations, don't use position here for X and Y, the transformation does that for you
            //also don't scale it or use origin, transformation does those things too          
            Rectangle spriteRectangle = CalculateBoundingRectangle(
                     new Rectangle(0, 0, Convert.ToInt32(FrameSize.X), Convert.ToInt32(FrameSize.Y)),
                     spriteTransform);

            // Calculate the bounding rectangle of the calling object in world space
            Rectangle thisRectangle = CalculateBoundingRectangle(
                     new Rectangle(0, 0, Convert.ToInt32(FrameSize.X), Convert.ToInt32(FrameSize.Y)),
                     thisTransform);

            // The per-pixel check is expensive, so check the bounding rectangles
            // first to prevent testing pixels when collisions are impossible.
            if (thisRectangle.Intersects(spriteRectangle))
            {
                // The color data for the images; used for per-pixel collision
                Color[] thisTextureData;        //calling object
                Color[] spriteTextureData;		//passed object

                // Extract collision data from calling object
                thisTextureData =
                    new Color[(int)(FrameSize.X * FrameSize.Y)];
                //get the rectangle that matches the current frame
                Rectangle frameRectangle = new Rectangle((int)(CurrentFrame.X * FrameSize.X),
                    (int)(CurrentFrame.Y * FrameSize.Y), (int)(FrameSize.X), (int)(FrameSize.Y));
                //get colour data for just that rectangle
                TextureImage.GetData(0, frameRectangle, thisTextureData, 0,
                    (int)(FrameSize.X * FrameSize.Y));

                // Extract collision data from passed object
                spriteTextureData =
                    new Color[(int)(sprite.FrameSize.X * sprite.FrameSize.Y)];
                //get the rectangle that matches the current frame
                Rectangle spriteFrameRectangle = new Rectangle((int)(CurrentFrame.X * FrameSize.X),
                    (int)(CurrentFrame.Y * FrameSize.Y), (int)(FrameSize.X), (int)(FrameSize.Y));
                //get colour data for just that rectangle
                sprite.TextureImage.GetData(0, spriteFrameRectangle, spriteTextureData, 0, 
                    (int)(FrameSize.X * FrameSize.Y));

                // Check collision at the pixel level
                if (IntersectPixels(spriteTransform, (int)(sprite.FrameSize.X),
                        (int)(sprite.FrameSize.Y), spriteTextureData,
                        thisTransform, (int)(FrameSize.X),
                        (int)(FrameSize.Y), thisTextureData
                        ))
                {
                    //if per pixel is true, return true from the method
                    hit = true;
                }
            }
            //this will be false if there was no rectangle collision or if
            //there was a rectangle collision, but no per pixel collision 
            return hit;
        }
    }
}
