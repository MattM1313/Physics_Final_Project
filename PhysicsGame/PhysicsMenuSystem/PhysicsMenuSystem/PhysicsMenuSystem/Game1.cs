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

namespace PhysicsMenuSystem
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        enum GameState
        {
            Launch,
            Menu,
            Options,
            Game,
            GameOver
        }
        public GameState gameState;
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }
        protected override void Initialize()
        {
            gameState = GameState.Launch;
            base.Initialize();
        }
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
        }
        protected override void UnloadContent()
        {
        }
        protected override void Update(GameTime gameTime)
        {
            switch (gameState)
            {
                case GameState.Launch:
                    UpdateLaunch(gameTime);
                    break;
                case GameState.Menu:
                    UpdateMenu(gameTime);
                    break;
                case GameState.Options:
                    UpdateOptions(gameTime);
                    break;
                case GameState.Game:
                    UpdateGame(gameTime);
                    break;
                case GameState.GameOver:
                    UpdateGameOver(gameTime);
                    break;
            }
            base.Update(gameTime);
        }

        private void UpdateLaunch(GameTime gameTime)
        {
        }

        private void UpdateMenu(GameTime gameTime)
        {
        }

        private void UpdateOptions(GameTime gameTime)
        {
        }

        private void UpdateGame(GameTime gameTime)
        {
        }

        private void UpdateGameOver(GameTime gameTime)
        {
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            switch (gameState)
            {
                case GameState.Launch:
                    DrawLaunch(spriteBatch);
                    break;
                case GameState.Menu:
                    DrawMenu(spriteBatch);
                    break;
                case GameState.Options:
                    DrawOptions(spriteBatch);
                    break;
                case GameState.Game:
                    DrawGame(spriteBatch);
                    break;
                case GameState.GameOver:
                    DrawGameOver(spriteBatch);
                    break;
            }
            base.Draw(gameTime);
        }

        private void DrawLaunch(SpriteBatch spriteBatch)
        {
        }

        private void DrawMenu(SpriteBatch spriteBatch)
        {
        }

        private void DrawOptions(SpriteBatch spriteBatch)
        {
        }

        private void DrawGame(SpriteBatch spriteBatch)
        {
        }

        private void DrawGameOver(SpriteBatch spriteBatch)
        {
        }
    }
}
