using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using EECEBlockBreaker.Interfaces;

namespace EECEBlockBreaker.Scenes
{
    class WelcomeScene : IScene
    {
        SpriteFont font;
        SpriteFont hsfont;

        public WelcomeScene()
        {
            Completed = false;
        }

        /// 
        /// If the scene has completed and is ready to be swapped out.
        /// 
        public bool Completed
        {
            get;
            private set;
        }

        /// 
        /// Loads the fonts required by the scene.
        /// 
        /// <param name="content">The content manager.</param>
        public void Load(ContentManager content)
        {
            hsfont = content.Load<SpriteFont>("hsfont");
            font = content.Load<SpriteFont>("font");
        }

        /// 
        /// Updates the scene (just checks if we can transition).
        /// 
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                Completed = true;
            }
        }

        /// 
        /// Draws the scene to the spritebatch.
        /// 
        /// <param name="sb"></param>
        public void Draw(SpriteBatch sb)
        {
            sb.DrawString(hsfont, "Block Breaker", new Vector2(90, 130), Color.Red);
            sb.DrawString(hsfont, "  The Game", new Vector2(90, 185), Color.Red);
            sb.DrawString(font, "Press the spacebar to start!", new Vector2(90, 280), Color.WhiteSmoke);
            sb.DrawString(font, "      By Richard Kim", new Vector2(90, 370), Color.WhiteSmoke);
        }
    }
}
