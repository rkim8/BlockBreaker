using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using EECEBlockBreaker.Interfaces;
using EECEBlockBreaker.Scenes;

namespace EECEBlockBreaker
{
    /// 
    /// This is the main type for your game
    /// 
    public class Game : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Player player = new Player();
        int nextLevel = 1;
        IScene scene;

        public Game()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            Window.Title = "Block Breaker";

            graphics.PreferredBackBufferWidth = 460;
            graphics.PreferredBackBufferHeight = 415;
        }

        /// 
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// 
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            scene = new WelcomeScene();// new GameScene(player, level);
            scene.Load(Content);

            // TODO: use this.Content to load your game content here
        }

        /// 
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// 
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();


            scene.Update(gameTime);

            if (scene.Completed)
            {
                // Check if they were successful.
                if (player.Lives > 0)
                {
                    // If successful, a new level will load
                    Level lvl = Level.FromFile(@"levels/" + nextLevel++ + ".txt");
                    if (lvl != null)
                    {
                        scene = new GameScene(player, lvl);
                        scene.Load(Content);
                    }
                    else
                    {
                        // Once game is finished, the highscores will be displayed.
                        nextLevel = 1;
                        scene = new HighScoreScene(player);
                        scene.Load(Content);
                        player = new Player();
                    }
                }
                else
                {
                    // They lost, display highscores.
                    nextLevel = 1;
                    scene = new HighScoreScene(player);
                    scene.Load(Content);
                    player = new Player();
                }
            }
            
            base.Update(gameTime);
        }

        /// 
        /// This is called when the game should draw itself.
        /// 
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();
            scene.Draw(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
