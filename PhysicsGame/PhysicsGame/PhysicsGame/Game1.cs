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

        List<Barrier> barrierList = new List<Barrier>();
        List<Enemy> enemyList = new List<Enemy>();
        


        //---Player---
        Tower tower;
        Player player;
        Texture2D towerTex;
        Texture2D playerTex;
        //------------

        //--Shooting--
        //SpriteFromSheet or SpriteWithAnimations
        Texture2D arrowTex, ballTex, fireballTex;
        Sprite arrow, ball, fireball;
        List<Sprite> shootList = new List<Sprite>();
        float fireDelay = FIRE_DELAY;
        const float FIRE_DELAY = 150f;
        float barrierDelay = BARRIER_DELAY;
        const float BARRIER_DELAY = 1000f;
        float spawnDelay = SPAWN_DELAY;
        const float SPAWN_DELAY = 3000f;

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
            towerTex = Content.Load<Texture2D>("tower");
            tower = new Tower(towerTex, new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height  - (towerTex.Height/4) ),
                 .5f, SpriteEffects.None, 100);
            playerTex = Content.Load<Texture2D>("player");


            player = new Player(playerTex, new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height - 29), new Vector2(0, 0), true,
                0f, 1f, SpriteEffects.None, new Vector2(32, 33), new Vector2(0, 0), new Vector2(1, 0), 1f, 10, 0, 5);



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


           
                for (int j = 0; j < enemyList.Count; j++)
                {
                    enemyList[j].Update(gameTime);
                    for(int b = 0; b < barrierList.Count; b++)
                    {
                        if (enemyList[j].CollisionRectangle.Intersects(barrierList[b].CollisionRectangle))
                            {
                            enemyList.RemoveAt(j);
                            barrierList[b].Health--;
                            if (barrierList[b].Health <= 0)
                            {
                                barrierList.RemoveAt(b);
                                break;
                            }
                            break;
                        }
                    }
                        
                     
                    foreach(Sprite s in shootList)
                    {
                        if (enemyList[j].CollisionRectangle.Intersects(s.CollisionRectangle))
                        {
                            enemyList.RemoveAt(j);
                            break;

                        }
                    }
                 }
                
            
            fireDelay -= elapsed;
            barrierDelay -= elapsed;
            spawnDelay -= elapsed;
            //Console.WriteLine(elapsed);
            
            player.Update(gameTime, GraphicsDevice);

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
            spawnEnemies();

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

           

             //Primitives2D.DrawRectangle(spriteBatch, new Rectangle(0, 0, 50, 50), Color.Black);

            tower.Draw(gameTime, spriteBatch);
            foreach (Barrier b in barrierList)
            {
                b.Draw(gameTime, spriteBatch);
            }
            foreach (Enemy e in enemyList)
            {
                e.Draw(gameTime, spriteBatch);
            }

            player.Draw(gameTime, spriteBatch);
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

            string bd = barrierDelay.ToString();
            string space = "Space to fire";

            ForegroundBatch.DrawString(CourierNew, bd, new Vector2(1200, 10), Color.Black); 
            
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
            bool keyPressed2 = false;

            player.OriginalVelocity = new Vector2(25);
            
            KeyboardState keyState = Keyboard.GetState();
            GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);
            GamePadState gamePadState2 = GamePad.GetState(PlayerIndex.Two);


               if (keyState.IsKeyDown(Keys.Escape)
                   || gamePadState.Buttons.Back == ButtonState.Pressed
                   || gamePadState2.Buttons.Back == ButtonState.Pressed)
               {
                   Exit();
               }

            //if (keyState.IsKeyDown(Keys.Up)
            //  || gamePadState.DPad.Up == ButtonState.Pressed
            //  || Math.Abs(gamePadState.ThumbSticks.Left.Y) > 0)
            //{
            //    //player1.Up();
            //    keyPressed = true;
            //    cam.Move(new Vector2(0, 3));
            //}
            
            if (keyState.IsKeyDown(Keys.Left)
                || gamePadState.DPad.Left == ButtonState.Pressed
                || Math.Abs(gamePadState.ThumbSticks.Left.X) < 0)
            {
                
                player.Left();
            }

            if (keyState.IsKeyDown(Keys.Right)
               || gamePadState.DPad.Right == ButtonState.Pressed
               || Math.Abs(gamePadState.ThumbSticks.Left.X) > 0)
            {
                
                player.Right();
            }

            if (keyState.IsKeyDown(Keys.RightShift)
               || gamePadState.Buttons.RightShoulder == ButtonState.Pressed)
              
            {
                //Kick Method
            }


            if (keyState.IsKeyDown(Keys.RightControl)
             || gamePadState.Buttons.LeftShoulder == ButtonState.Pressed)
            {
                if (barrierDelay <= 0f)
                {

                    createBarrier();
                    barrierDelay = BARRIER_DELAY;
                }
              
 
            }
            if (!keyPressed)
            {
                player.Idle();
            }

            MouseState currMouseState = Mouse.GetState();

            if (currMouseState.X != prevMouseState.X ||
                currMouseState.Y != prevMouseState.Y)
            {
                //player.Rotation
                Vector2 mouseLoc = new Vector2(currMouseState.X, currMouseState.Y);
                Vector2 direction = (tower.Position) - mouseLoc;
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



            if (currMouseState.LeftButton == ButtonState.Pressed)
            {
               if(fireDelay <= 0f)
               {
                    switch (shootState)
                    {
                        case ShootingState.Ball:
                            Sprite ballShot = new Sprite(ballTex, new Vector2(tower.Position.X - 5, tower.Position.Y),
                                        new Vector2((float)Math.Cos((angle)), (float)Math.Sin((angle))) * 400f, true, 0, 1f, SpriteEffects.None);


                            shootList.Add(ballShot);

                            break;


                        case ShootingState.Fireball:
                            Sprite fireballShot = new Sprite(ballTex, new Vector2(tower.Position.X - 5, tower.Position.Y),
                                        new Vector2((float)Math.Cos((angle)), (float)Math.Sin((angle))) * 450f, true, 0, 1.5f, SpriteEffects.None);


                            shootList.Add(fireballShot);

                            break;

                        case ShootingState.Arrow:
                            Sprite arrowShot = new Sprite(arrowTex, new Vector2(tower.Position.X - 5, tower.Position.Y),
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




        private void createBarrier()
        {

            Barrier barrier = new Barrier(Content.Load<Texture2D>("barrier"), player.Position, Vector2.Zero, true, 0f, 1f, SpriteEffects.None, 1f, 5);
            
            barrierList.Add(barrier);



        }

        private void spawnEnemies()
        {
            if (spawnDelay <= 0f)
            {
                Enemy enemy = new Enemy(Content.Load<Texture2D>("enemy"), new Vector2(0, GraphicsDevice.Viewport.Height - 29), new Vector2(50, 0),
                    true, 0f, 1f, SpriteEffects.None, new Vector2(48, 48), new Vector2(0, 0), new Vector2(1, 0), 1f, 5, 1, 1);

                enemyList.Add(enemy);
                spawnDelay = SPAWN_DELAY;
            } 
        }


    }//End Class
}
