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
        Tower player1;
        Texture2D player1Tex;
        //------------

        //--Shooting--
        //SpriteFromSheet or SpriteWithAnimations
        Texture2D arrowTex, ballTex, fireballTex;
        Sprite arrow, ball, fireball;
        List<Sprite> shootList = new List<Sprite>();
        float fireDelay = FIRE_DELAY;
        const float FIRE_DELAY = 150f;

        //------------


        //---Camera---
        Camera cam;
        
        //cam.Pos = new Vector2(500.0f,200.0f);


        //----------


        MouseState prevMouseState;
        float angle;


        enum ShootingState
        {
            Arrow,
            Ball,
            Fireball

        }



        //-----Font----
        SpriteBatch ForegroundBatch;
        SpriteFont CourierNew;
        Vector2 FontPos;
        
        //------------





        ShootingState shootState = ShootingState.Ball;
        

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferHeight = 720;
            graphics.PreferredBackBufferWidth = 1280;
            //graphics.IsFullScreen = true;

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

            cam = new Camera();
            cam.Pos = new Vector2(500f, 200f);







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
            arrowTex = Content.Load<Texture2D>("Arrow");
            
            //-----------
            player1Tex = Content.Load<Texture2D>("tower");
            player1 = new Tower(player1Tex, new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height  - (player1Tex.Height/4) ),
                 .5f, SpriteEffects.None, 100);



            //-----Font--------------
            CourierNew = Content.Load<SpriteFont>("Courrier");
            ForegroundBatch = new SpriteBatch(graphics.GraphicsDevice);
            FontPos = new Vector2(graphics.GraphicsDevice.Viewport.Width / 2, 
                graphics.GraphicsDevice.Viewport.Height / 2);
            



            //-----------------------






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
            

            float elapsed = (float)gameTime.ElapsedGameTime.TotalMilliseconds;
          
            fireDelay -= elapsed;
            Console.WriteLine(elapsed);

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
            doPhysics();

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            
            /*spriteBatch.Begin(SpriteSortMode.BackToFront,
                        BlendState.AlphaBlend,
                        null,
                        null,
                        null,
                        null,
                        cam.get_transformation(GraphicsDevice));*/
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
            //Font spriteBatch
            ForegroundBatch.Begin();
               
            // Draw instructions
            Color c = new Color(0, 0, 0);
            string arrow = "Q = Arrows";
            string ball = "W = CannonBalls";
            string fireball = "E = FireBalls";

            string space = "Space to fire";
           
            
            if (shootState == ShootingState.Arrow)
            {
                ForegroundBatch.DrawString(CourierNew, arrow, new Vector2(10, 10), Color.Red);
                
            }
            else
            {
                ForegroundBatch.DrawString(CourierNew, arrow, new Vector2(10, 10), Color.Black);
                
            }


            if (shootState == ShootingState.Ball)
            {
                ForegroundBatch.DrawString(CourierNew, ball, new Vector2(10, 30), Color.Red);
            }
            else
            {
                ForegroundBatch.DrawString(CourierNew, ball, new Vector2(10, 30), Color.Black);
            }

            if (shootState == ShootingState.Fireball)
            {
                ForegroundBatch.DrawString(CourierNew, fireball, new Vector2(10, 50), Color.Red);
            }
            else
            {
                ForegroundBatch.DrawString(CourierNew, fireball, new Vector2(10, 50), Color.Black);
            }

            ForegroundBatch.DrawString(CourierNew, space, new Vector2(10, 400), c);
            ForegroundBatch.End();




            base.Draw(gameTime);
        }

        private void updateInput()
        {
            bool keyPressed = false;


            
            KeyboardState keyState = Keyboard.GetState();
            GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);

               if (keyState.IsKeyDown(Keys.Escape))
               {
                   Exit();
               }

            if (keyState.IsKeyDown(Keys.Up)
              || keyState.IsKeyDown(Keys.W)
              || gamePadState.DPad.Up == ButtonState.Pressed
              || Math.Abs(gamePadState.ThumbSticks.Left.Y) > 0)
            {
                //player1.Up();
                keyPressed = true;
                cam.Move(new Vector2(0, 3));
            }
            if (keyState.IsKeyDown(Keys.Down)
              || keyState.IsKeyDown(Keys.S)
              || gamePadState.DPad.Down == ButtonState.Pressed
              || gamePadState.ThumbSticks.Left.Y < -0.5f)
            {
                //player.Down();
                keyPressed = true;
                cam.Move(new Vector2(0, -3));
            }
            if (keyState.IsKeyDown(Keys.Left)
              || keyState.IsKeyDown(Keys.A)
              || gamePadState.DPad.Left == ButtonState.Pressed
              || gamePadState.ThumbSticks.Left.X < -0.5f)
            {
                //player.Left();
                keyPressed = true;
                cam.Move(new Vector2(3, 0));
            }
            if (keyState.IsKeyDown(Keys.Right)
              || keyState.IsKeyDown(Keys.D)
              || gamePadState.DPad.Right == ButtonState.Pressed
              || gamePadState.ThumbSticks.Left.X > 0.5f)
            {
                ///player.Right();
                keyPressed = true;
                cam.Move(new Vector2(-3,0));
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
               // cam.Zoom -= 0.1f;
            }

            if (keyState.IsKeyDown(Keys.W))
            {
                shootState = ShootingState.Ball;
               // cam.Zoom += 0.1f;
            }

            if (keyState.IsKeyDown(Keys.E))
            {
                shootState = ShootingState.Fireball;
                //cam.Rotation += 0.5f;
            }



            if (keyState.IsKeyDown(Keys.Space))
            {
               if(fireDelay <= 0f)
               {
                    switch (shootState)
                    {
                        case ShootingState.Ball:
                            Sprite ballShot = new Sprite(ballTex, new Vector2(player1.Position.X - 5, player1.Position.Y),
                                        new Vector2((float)Math.Cos((angle)), (float)Math.Sin((angle))) * 400f, true, 0, 1f, SpriteEffects.None);


                            shootList.Add(ballShot);

                            break;


                        case ShootingState.Fireball:
                            Sprite fireballShot = new Sprite(ballTex, new Vector2(player1.Position.X - 5, player1.Position.Y),
                                        new Vector2((float)Math.Cos((angle)), (float)Math.Sin((angle))) * 450f, true, 0, 1.5f, SpriteEffects.None);


                            shootList.Add(fireballShot);

                            break;

                        case ShootingState.Arrow:
                            Sprite arrowShot = new Sprite(arrowTex, new Vector2(player1.Position.X - 5, player1.Position.Y),
                                        new Vector2((float)Math.Cos((angle)), (float)Math.Sin((angle))) * 475f, true, 0, .1f, SpriteEffects.None);



                            //arrowShot.Rotation = angle + (float)45.5;
                            arrowShot.Rotation += (float)Math.Atan2(arrowShot.Velocity.X * 10, -arrowShot.Velocity.Y * 10);



                            shootList.Add(arrowShot);

                            break;
                    }
                }
                prevMouseState = currMouseState;
                fireDelay = FIRE_DELAY;
        }
            
        }//End updateInput

        private void doPhysics()
        {
            switch (shootState)
            {
                case ShootingState.Arrow:
                    //---Arrow-Physics--
                    foreach (Sprite s in shootList)
                    {
                        s.Velocity += new Vector2(0, 5);
                        s.Rotation = (float)Math.Atan2(s.Velocity.X, -s.Velocity.Y);


                       

                    }


                    break;

                case ShootingState.Ball:
                    //---Ball-Physics--
                    foreach (Sprite s in shootList)
                    {
                        s.Velocity += new Vector2(0, 5);



                     
                    }

                    break;

                case ShootingState.Fireball:
                    //---Fireball-Physics--
                    foreach (Sprite s in shootList)
                    {
                        s.Velocity += new Vector2(0, 5);
                    }

                    break;



            }



        }


    }//End Class
}
