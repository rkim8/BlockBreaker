using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace EECEBlockBreaker
{
    class PowerUp : Interfaces.IGameObject
    {
        public enum PowerUpType 
        { 
            SmallPaddle, 
            BigPaddle, 
            FastBall, 
            ExtraBall, 
            PlusLife 
        }
       
        Texture2D texture;
        Vector2 position =  Vector2.Zero;

        /// 
        /// The type of the power up.
        /// 
        public PowerUpType Type
        {
            get;
            private set;
        }

        // The center of the power up.
        public Vector2 Center
        {
            get
            {
                return new Vector2(position.X + texture.Width / 2, position.Y + texture.Height / 2);
            }
        }

        /// 
        /// The power up collision bounds.
        /// 
        public Rectangle Bounds
        {
            get
            {
                return new Rectangle((int)position.X, (int)position.Y, (int) texture.Width, (int)texture.Height);
            }
        }

        /// 
        /// Constructor for the PowerUp class.
        /// 
        /// <param name="p">Position of the Power Up.</param>
        /// <param name="type">Number ID of the power up kind.</param>
        public PowerUp(Vector2 p, PowerUpType type)
        {
            position = p;
            Type = type;
        }

        /// 
        /// Loads the appropriate sprite and sets the appropriate type of the Power Up using the power up ID.
        /// 
        /// <param name="content">Project Content.</param>
        public void Load(ContentManager content)
        {
            switch (Type)
            {
                case PowerUpType.BigPaddle:
                    texture = content.Load<Texture2D>("BiggerPaddle");
                    break;
                case PowerUpType.SmallPaddle:
                    texture = content.Load<Texture2D>("SmallerPaddle");
                    break;
                case PowerUpType.ExtraBall:
                    texture = content.Load<Texture2D>("ExtraBall");
                    break;
                case PowerUpType.FastBall:
                    texture = content.Load<Texture2D>("FasterBall");
                    break;
                case PowerUpType.PlusLife:
                    texture = content.Load<Texture2D>("Life");
                    break;
            }
        }

        /// 
        /// Updates the Y-coordinate position of the power up as game time passes.
        /// 
        /// <param name="gameTime">Game time</param>
        public void Update(GameTime gameTime)
        {
            position.Y += (float)gameTime.ElapsedGameTime.TotalSeconds * 200;
        }

        /// 
        /// Draw the powerup to the sprite batch.
        /// 
        /// <param name="sb"></param>
        public void Draw(SpriteBatch sb)
        {
            sb.Draw(texture, new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height), Color.White);
        }


    }
}
