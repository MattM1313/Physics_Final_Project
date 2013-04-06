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
    public class Sprite
    {
        // This variable will hold our position - make it a property so game class
        //can use it to change position when mouse moved
        protected Vector2 position;
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }
        public Texture2D TextureImage { get; set; }
        //make fields protected so derived classes can change them
        //origin of sprite, either null or the center of the image        
        protected Vector2 spriteOrigin;
        public Vector2 SpriteOrigin
        {
            get { return spriteOrigin; }
            set { spriteOrigin = value; }
        }
        public bool SetOrigin { get; set; }

        //vector so it has independant x and y values
        protected Vector2 velocity;
        public Vector2 Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }
        //velocity set in constructor, needs a 
        //separate property so it doesn't get zeroed
        //when sprite idles
        protected Vector2 originalVelocity;
        public Vector2 OriginalVelocity
        {
            get { return originalVelocity; }
            set { originalVelocity = value; }
        }
        //current rotation value
        public float Rotation { get; set; }
        //speed of rotation of the sprite
        public float RotationSpeed { get; set; }
        public float Scale { get; set; }
        //for power ups/downs, keep original scale and velocity so we can go back
        //to these values after power up is done
        protected float OriginalScale { get; set; }
        public SpriteEffects SpriteEffect { get; set; }

        //sound to be made when sprite collides with something
        public SoundEffect CollisionCueName { get; private set; }
        // Value to add to player score when this sprite is hit (or avoided)
        public int ScoreValue { get; protected set; }

        //is he active or not (should he be updated and drawn?)
        public bool Active { get; set; }

        //rectangle occupied by texture - bounding rectangle
        public virtual Rectangle CollisionRectangle
        {
            get
            {
                return new Rectangle((int)(position.X - SpriteOrigin.X * Scale), (int)(position.Y - SpriteOrigin.Y * Scale),
                   (int)(TextureImage.Width * Scale), (int)(TextureImage.Height * Scale));
            }
        }

        // base version
        public Sprite(Texture2D textureImage, Vector2 position, Vector2 velocity, bool setOrigin,
            float rotationSpeed, float scale, SpriteEffects spriteEffect, SoundEffect collisionCueName, int scoreValue)
        {
            Position = position;
            TextureImage = textureImage;
            OriginalVelocity = velocity;
            Velocity = velocity;
            SetOrigin = setOrigin;
            if (SetOrigin)
            {
                SpriteOrigin = new Vector2(TextureImage.Width / 2, TextureImage.Height / 2);
            }
            RotationSpeed = rotationSpeed;
            Scale = scale;
            OriginalScale = scale;
            SpriteEffect = spriteEffect;
            CollisionCueName = collisionCueName;
            ScoreValue = scoreValue;

            Active = true;
        }

        //version that does not keep sprite on screen
        public virtual void Update(GameTime gameTime)
        {
            if (Active)
            {
                // time between frames
                float timeLapse = (float)(gameTime.ElapsedGameTime.Milliseconds / 100.0f);
                //move the sprite
                position += Velocity * timeLapse;
                
                // Scale radians by time between frames so rotation is uniform
                // rate on all systems. Cap between 0 & 2PI for full rotation.
                Rotation += RotationSpeed * timeLapse;
                Rotation = Rotation % (MathHelper.Pi * 2.0f);
            }
        }
        //version that keeps sprite on screen
        public virtual void Update(GameTime gameTime, GraphicsDevice Device)
        {
            if (Active)
            {
                //call overload to do rotation and basic movement
                Update(gameTime);
                //keep on screen
                if (Position.X > Device.Viewport.Width - (TextureImage.Width - SpriteOrigin.X) * Scale)
                {
                    position.X = Device.Viewport.Width - (TextureImage.Width - SpriteOrigin.X) * Scale;
                    velocity.X = -Velocity.X;
                }
                else if (Position.X < SpriteOrigin.X * Scale)
                {
                    position.X = SpriteOrigin.X * Scale;
                    velocity.X = -Velocity.X;
                }

                if (Position.Y > Device.Viewport.Height - (TextureImage.Height - SpriteOrigin.Y) * Scale)
                {
                    position.Y = Device.Viewport.Height - (TextureImage.Height - SpriteOrigin.Y) * Scale;
                    velocity.Y = -Velocity.Y;
                }
                else if (Position.Y < SpriteOrigin.Y * Scale)
                {
                    position.Y = SpriteOrigin.Y * Scale;
                    velocity.Y = -Velocity.Y;
                }
            }
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (Active)
            {
                spriteBatch.Draw(TextureImage,
                     position,
                     null,
                     Color.White,
                     Rotation,
                     SpriteOrigin,
                     Scale,
                     SpriteEffect,
                     0);
                }
        }

        //is there a collision with another sprite?
        public virtual bool CollisionSprite(Sprite sprite)
        {
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
                     new Rectangle(0, 0, Convert.ToInt32(TextureImage.Width), Convert.ToInt32(TextureImage.Height)),
                     spriteTransform);

            // Calculate the bounding rectangle of the calling object in world space
            Rectangle thisRectangle = CalculateBoundingRectangle(
                     new Rectangle(0, 0, Convert.ToInt32(TextureImage.Width), Convert.ToInt32(TextureImage.Height)),
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
                    new Color[TextureImage.Width * TextureImage.Height];
                TextureImage.GetData(thisTextureData);

                // Extract collision data from passed object
                spriteTextureData =
                    new Color[sprite.TextureImage.Width * sprite.TextureImage.Height];
                sprite.TextureImage.GetData(spriteTextureData);

                // Check collision 
                if (IntersectPixels(spriteTransform, sprite.TextureImage.Width,
                        sprite.TextureImage.Height, spriteTextureData,
                        thisTransform, TextureImage.Width,
                        TextureImage.Height, thisTextureData
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

        //is the mouse over the sprite?
        public bool CollisionMouse(int x, int y)
        {
            return CollisionRectangle.Contains(x, y);
        }

        // These match up with the Arrow keys
        public virtual void Up()
        {
            velocity.Y -= OriginalVelocity.Y;
        }

        public virtual void Down()
        {
            velocity.Y += OriginalVelocity.Y;
        }

        public virtual void Right()
        {
            velocity.X += OriginalVelocity.X;
        }

        public virtual void Left()
        {
            velocity.X -= OriginalVelocity.X;
        }

        public virtual void Idle()
        {
            Velocity *= .95f;
        }

        //determine if sprite has gone off screen
        public virtual bool IsOffScreen(GraphicsDevice Device)
        {
            if (position.X < -(TextureImage.Width - SpriteOrigin.X) * Scale ||
                position.X > Device.Viewport.Width + (TextureImage.Width - SpriteOrigin.X) * Scale ||
                position.Y < -(TextureImage.Height - SpriteOrigin.Y) * Scale ||
                position.Y > Device.Viewport.Height + (TextureImage.Height - SpriteOrigin.Y) * Scale)
            {
                return true;
            }
            return false;
        }

        public void SetMoveLeft()
        {
            SpriteEffect = SpriteEffects.None;
        }

        public void SetMoveRight()
        {
            SpriteEffect = SpriteEffects.FlipHorizontally;
        }

        //for power ups/downs
        public void ResetScale()
        {
            Scale = OriginalScale;
        }
        public void ResetSpeed()
        {
            velocity = OriginalVelocity;
        }
        public void ModifyScale(float modifier)
        {
            Scale *= modifier;
        }
        public void ModifySpeed(float modifier)
        {
            velocity *= modifier;
        }

        /// <summary>
        /// Calculates an axis aligned rectangle which fully contains an arbitrarily
        /// transformed axis aligned rectangle.
        /// </summary>
        /// <param name="rectangle">Original bounding rectangle.</param>
        /// <param name="transform">World transform of the rectangle.</param>
        /// <returns>A new rectangle which contains the trasnformed rectangle.</returns>
        public static Rectangle CalculateBoundingRectangle(Rectangle rectangle,
                                                Matrix transform)
        {
            // Get all four corners in local space
            Vector2 leftTop = new Vector2(rectangle.Left, rectangle.Top);
            Vector2 rightTop = new Vector2(rectangle.Right, rectangle.Top);
            Vector2 leftBottom = new Vector2(rectangle.Left, rectangle.Bottom);
            Vector2 rightBottom = new Vector2(rectangle.Right, rectangle.Bottom);

            // Transform all four corners into work space
            Vector2.Transform(ref leftTop, ref transform, out leftTop);
            Vector2.Transform(ref rightTop, ref transform, out rightTop);
            Vector2.Transform(ref leftBottom, ref transform, out leftBottom);
            Vector2.Transform(ref rightBottom, ref transform, out rightBottom);

            // Find the minimum and maximum extents of the rectangle in world space
            Vector2 min = Vector2.Min(Vector2.Min(leftTop, rightTop),
                                      Vector2.Min(leftBottom, rightBottom));
            Vector2 max = Vector2.Max(Vector2.Max(leftTop, rightTop),
                                      Vector2.Max(leftBottom, rightBottom));

            // Return as a rectangle
            return new Rectangle((int)min.X, (int)min.Y,
                        (int)(max.X - min.X), (int)(max.Y - min.Y));
        }

        /// <summary>
        /// Determines if there is overlap of the non-transparent pixels between two
        /// sprites.
        /// </summary>
        /// <param name="transformA">World transform of the first sprite.</param>
        /// <param name="widthA">Width of the first sprite's texture.</param>
        /// <param name="heightA">Height of the first sprite's texture.</param>
        /// <param name="dataA">Pixel color data of the first sprite.</param>
        /// <param name="transformB">World transform of the second sprite.</param>
        /// <param name="widthB">Width of the second sprite's texture.</param>
        /// <param name="heightB">Height of the second sprite's texture.</param>
        /// <param name="dataB">Pixel color data of the second sprite.</param>
        /// <returns>True if non-transparent pixels overlap; false otherwise</returns>
        public static bool IntersectPixels(
                        Matrix transformA, int widthA, int heightA, Color[] dataA,
                        Matrix transformB, int widthB, int heightB, Color[] dataB)
        {
            // Calculate a matrix which transforms from A's local space into
            // world space and then into B's local space
            Matrix transformAToB = transformA * Matrix.Invert(transformB);

            // When a point moves in A's local space, it moves in B's local space with a
            // fixed direction and distance proportional to the movement in A.
            // This algorithm steps through A one pixel at a time along A's X and Y axes
            // Calculate the analogous steps in B:
            Vector2 stepX = Vector2.TransformNormal(Vector2.UnitX, transformAToB);
            Vector2 stepY = Vector2.TransformNormal(Vector2.UnitY, transformAToB);

            // Calculate the top left corner of A in B's local space
            // This variable will be reused to keep track of the start of each row
            Vector2 yPosInB = Vector2.Transform(Vector2.Zero, transformAToB);

            // For each row of pixels in A
            for (int yA = 0; yA < heightA; yA++)
            {
                // Start at the beginning of the row
                Vector2 posInB = yPosInB;

                // For each pixel in this row
                for (int xA = 0; xA < widthA; xA++)
                {
                    // Round to the nearest pixel
                    int xB = (int)Math.Round(posInB.X);
                    int yB = (int)Math.Round(posInB.Y);

                    // If the pixel lies within the bounds of B
                    if (0 <= xB && xB < widthB &&
                        0 <= yB && yB < heightB)
                    {
                        // Get the colors of the overlapping pixels
                        Color colorA = dataA[xA + yA * widthA];
                        Color colorB = dataB[xB + yB * widthB];

                        // If both pixels are not completely transparent,
                        if (colorA.A != 0 && colorB.A != 0)
                        {
                            // then an intersection has been found
                            return true;
                        }
                    }

                    // Move to the next pixel in the row
                    posInB += stepX;
                }

                // Move to the next row
                yPosInB += stepY;
            }

            // No intersection found
            return false;
        }

        //**replaced by new version that does it using transformations and matrices
        ////is there a collision with another sprite?
        //public bool CollisionSprite(Sprite sprite)
        //{
        //    bool hit = false;
        //    //test for rectangle collision. If true, do per pixel
        //    if (CollisionRectangle.Intersects(sprite.CollisionRectangle))
        //    {
        //        // The color data for the images; used for per-pixel collision
        //        Color[] thisTextureData;        //calling object
        //        Color[] spriteTextureData;		//passed object

        //        // Extract collision data from calling object
        //        thisTextureData =
        //            new Color[TextureImage.Width * TextureImage.Height];
        //        TextureImage.GetData(thisTextureData);
        //        // Extract collision data from passed object
        //        spriteTextureData =
        //            new Color[sprite.TextureImage.Width * sprite.TextureImage.Height];
        //        sprite.TextureImage.GetData(spriteTextureData);
        //        // Check collision with person
        //        if (IntersectPixels(CollisionRectangle, thisTextureData,
        //                    sprite.CollisionRectangle, spriteTextureData))
        //        {
        //            //if per pixel is true, return true from the method
        //            hit = true;
        //        }
        //    }
        //    //this will be false if there was no rectangle collision or if
        //    //there was a rectangle collision, but no per pixel collision 
        //    return hit;
        //}

        //**replaced by new version that does it using transformations and matrices
        ///// <summary>
        ///// Determines if there is overlap of the non-transparent pixels
        ///// between two sprites.
        ///// </summary>
        ///// <param name="rectangleA">Bounding rectangle of the first sprite</param>
        ///// <param name="dataA">Pixel data of the first sprite</param>
        ///// <param name="rectangleB">Bounding rectangle of the second sprite</param>
        ///// <param name="dataB">Pixel data of the second sprite</param>
        ///// <returns>True if non-transparent pixels overlap; false otherwise</returns>
        //static bool IntersectPixels(Rectangle rectangleA, Color[] dataA,
        //                            Rectangle rectangleB, Color[] dataB)
        //{
        //    // Find the bounds of the rectangle intersection
        //    int top = Math.Max(rectangleA.Top, rectangleB.Top);
        //    int bottom = Math.Min(rectangleA.Bottom, rectangleB.Bottom);
        //    int left = Math.Max(rectangleA.Left, rectangleB.Left);
        //    int right = Math.Min(rectangleA.Right, rectangleB.Right);

        //    // Check every point within the intersection bounds
        //    for (int y = top; y < bottom; y++)
        //    {
        //        for (int x = left; x < right; x++)
        //        {
        //            // Get the color of both pixels at this point
        //            Color colorA = dataA[(x - rectangleA.Left) +
        //                                 (y - rectangleA.Top) * rectangleA.Width];
        //            Color colorB = dataB[(x - rectangleB.Left) +
        //                                 (y - rectangleB.Top) * rectangleB.Width];

        //            // If both pixels are not completely transparent,
        //            if (colorA.A != 0 && colorB.A != 0)
        //            {
        //                // then an intersection has been found
        //                return true;
        //            }
        //        }
        //    }

        //    // No intersection found
        //    return false;
        //}
    }
}
