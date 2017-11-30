using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace EECEBlockBreaker
{
    class Ball : Interfaces.IGameObject
    {
        Texture2D texture;
        Vector2 ballPos = new Vector2(200, 340);
        double velocityCoeff;
        bool faster = false;


        /// 
        /// Constructor for the Ball class.
        /// 
        /// <param name="x">X-Coordinate of the ball</param>
        /// <param name="y">Y-Coordinate of the ball</param>
        public Ball(int x, int y)
        {
            ballPos.X = x;
            ballPos.Y = y;
        }

        /// 
        /// Loads the ball sprite from the project content.
        /// 
        /// <param name="content">Project Content</param>
        public void Load(ContentManager content)
        {
            texture = content.Load<Texture2D>("ball");
        }

        /// 
        /// The position of the ball.
        /// 
        public Vector2 Position {
            get
            {
                return ballPos;
            }
        }

        public void FasterBall()
        {
            faster = true;
        }

        private Vector2 velocity = -Vector2.One;
        /// 
        /// The velocity of the ball.
        /// 
        public Vector2 Velocity
        {
            get
            {
                return velocity;
            }
            set
            {
                velocity = value;
            }
        }

        /// 
        /// The radius of the ball.
        /// 
        public float Radius
        {
            get
            {
                return texture.Width / 2.0f;
            }
        }

        /// 
        /// Updates the position of the ball.
        /// Increases the speed of the ball if the bool 'faster' is true.
        /// 
        /// <param name="gameTime">Game time</param>
        public void Update(GameTime gameTime)
        {
            if (faster)
            {
                velocityCoeff = 1.5;
            }
            else
            {
                velocityCoeff = 1;
            }

            ballPos += velocity * (float)velocityCoeff * (float)gameTime.ElapsedGameTime.TotalSeconds * 150;
        }

        /// 
        /// Draws the ball to the spritebatch.
        /// 
        /// <param name="sb"></param>
        public void Draw(SpriteBatch sb)
        {
            sb.Draw(texture, new Rectangle((int)(Position.X - Radius), (int)(Position.Y - Radius), (int)Radius * 2, (int)Radius * 2), Color.Khaki);
        }
    }
}
