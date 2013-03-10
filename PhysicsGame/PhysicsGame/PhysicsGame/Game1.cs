using System;
using System.Collections.Generic;
using System.Linq;
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
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;


        //---Player---
        Sprite player1;
        Texture2D player1Tex;
        //------------

        //--Shooting--
        //SpriteFromSheet or SpriteWithAnimations
        Texture2D arrowTex, ballTex, fireballTex;
        Sprite arrow, ball, fireball;
        List<Sprite> shootList = new List<Sprite>();

        //------------


        MouseState prevMouseState;
        float angle;


        enum ShootingState
        {
            Arrow,
            Ball,
            Fireball

        }
        ShootingState shootState = ShootingState.Ball;
        

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";


            IsMouseVisible = true;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
         








            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);



            //-----------
            ballTex = Content.Load<Texture2D>("ball");
            fireballTex = Content.Load<Texture2D>("ball");

            //ball = new Sprite(ballTex, player1.Position, new Vector2((float)Math.Cos((angle)), (float)Math.Sin((angle))),
                //true, 0, 1f, SpriteEffects.None);



            //-----------

            player1Tex = Content.Load<Texture2D>("shield2_1");
            
            player1 = new Sprite(player1Tex, new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height - (player1Tex.Height / 2)), Vector2.Zero,
                true, 0, 1f, SpriteEffects.None);








        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            Console.WriteLine(shootList.Count);

            for (int i = 0; i < shootList.Count; i++)
            {

                shootList[i].Update(gameTime);


                if (shootList[i].Position.Y < 0f || shootList[i].Position.X < 0f || shootList[i].Position.X > graphics.GraphicsDevice.Viewport.Width
                || shootList[i].Position.Y > graphics.GraphicsDevice.Viewport.Height)
                {
                    shootList.RemoveAt(i);

                }


            }

            updateInput();


            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            spriteBatch.Begin();

            player1.Draw(gameTime, spriteBatch);
            foreach (Sprite shot in shootList)
            {
                switch (shootState)
                {
                    case ShootingState.Arrow:

                        shot.Draw(gameTime, spriteBatch);

                        break;
                    case ShootingState.Ball:

                        shot.Draw(gameTime, spriteBatch);

                        break;
                    case ShootingState.Fireball:

                        shot.Draw(gameTime, spriteBatch);

                        break;
                }

            }




            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void updateInput()
        {
            bool keyPressed = false;
            KeyboardState keyState = Keyboard.GetState();
            GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);

            if (keyState.IsKeyDown(Keys.Up)
              || keyState.IsKeyDown(Keys.W)
              || gamePadState.DPad.Up == ButtonState.Pressed
              || Math.Abs(gamePadState.ThumbSticks.Left.Y) > 0)
            {
                //player1.Up();
                keyPressed = true;
            }
            if (keyState.IsKeyDown(Keys.Down)
              || keyState.IsKeyDown(Keys.S)
              || gamePadState.DPad.Down == ButtonState.Pressed
              || gamePadState.ThumbSticks.Left.Y < -0.5f)
            {
                //player.Down();
                keyPressed = true;
            }
            if (keyState.IsKeyDown(Keys.Left)
              || keyState.IsKeyDown(Keys.A)
              || gamePadState.DPad.Left == ButtonState.Pressed
              || gamePadState.ThumbSticks.Left.X < -0.5f)
            {
                //player.Left();
                keyPressed = true;
            }
            if (keyState.IsKeyDown(Keys.Right)
              || keyState.IsKeyDown(Keys.D)
              || gamePadState.DPad.Right == ButtonState.Pressed
              || gamePadState.ThumbSticks.Left.X > 0.5f)
            {
                ///player.Right();
                keyPressed = true;
            }
            //if (!keyPressed)
            //{
            //    player.Idle();
            //}
           
            MouseState currMouseState = Mouse.GetState();

            if (currMouseState.X != prevMouseState.X ||
                currMouseState.Y != prevMouseState.Y)
            {
                //player.Rotation
                Vector2 mouseLoc = new Vector2(currMouseState.X, currMouseState.Y);

                
                Vector2 direction = (player1.Position) - mouseLoc; 
                angle = (float)(Math.Atan2(-direction.Y, -direction.X));

               
                //player1.Rotation = angle + (float)45.5;


            }

             if (keyState.IsKeyDown(Keys.Q))
             {
                 shootState = ShootingState.Arrow;
             }

             if (keyState.IsKeyDown(Keys.W))
             {
                 shootState = ShootingState.Ball;
             }

             if (keyState.IsKeyDown(Keys.E))
             {
                 shootState = ShootingState.Fireball;
             }



             if (keyState.IsKeyDown(Keys.Space))
             {
                switch(shootState)
                {
                    case ShootingState.Ball:
                Sprite ballShot = new Sprite(ballTex, new Vector2(player1.Position.X - 5, player1.Position.Y),
                            new Vector2((float)Math.Cos((angle)), (float)Math.Sin((angle))) * 400f, true, 0, 1f, SpriteEffects.None);
                
                
            shootList.Add(ballShot);

            break;


                    case ShootingState.Fireball:
            Sprite fireballShot = new Sprite(ballTex, new Vector2(player1.Position.X - 5, player1.Position.Y),
                        new Vector2((float)Math.Cos((angle)), (float)Math.Sin((angle))) * 600f, true, 0, 1.5f, SpriteEffects.None);


           shootList.Add(fireballShot);

            break;
            }
        }
            prevMouseState = currMouseState;
        }


    }
}
