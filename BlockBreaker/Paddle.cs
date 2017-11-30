using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace EECEBlockBreaker
{
    class Paddle : Interfaces.IGameObject, Interfaces.ICollidable
    {
        Texture2D texture, SmallTexture, NormalTexture, BigTexture;
        Vector2 position = new Vector2(180, 395);
        double rBound;
        enum PaddleSize { Small, Normal, Big };
        PaddleSize size = PaddleSize.Normal;

        /// 
        /// Loads the different paddle sprites from the solution content.
        /// 
        /// <param name="content">Project Content</param>
        public void Load(ContentManager content)
        {
            NormalTexture = content.Load<Texture2D>("NormalPaddle");
            SmallTexture = content.Load<Texture2D>("SmallPaddle");
            BigTexture = content.Load<Texture2D>("BigPaddle");
            texture = NormalTexture;
        }

        /// 
        /// Makes the paddle smaller by switching to a smaller sprite.
        /// 
        public void MakeSmaller()
        {
            if (size == PaddleSize.Big)
            {
                texture = NormalTexture;
                size = PaddleSize.Normal;
            }

            else if (size == PaddleSize.Normal)
            {
                texture = SmallTexture;
                size = PaddleSize.Small;
            }

        }

        /// 
        /// Makes the paddle wider by switching to a bigger sprite.
        /// 
        public void MakeBigger()
        {
            if (size == PaddleSize.Small)
            {
                texture = NormalTexture;
                size = PaddleSize.Normal;
            }

            else if (size == PaddleSize.Normal)
            {
                texture = BigTexture;
                size = PaddleSize.Big;
            }
        }

        /// 
        /// Updates the paddles position.
        /// 
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            rBound = 460 - texture.Width;

            // Moves the paddle to the left.
            if (Keyboard.GetState().IsKeyDown(Keys.A) || Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                if (position.X >= 0)
                    position.X -= 7;
            }

            // Moves the paddle to the right.
            if (Keyboard.GetState().IsKeyDown(Keys.D) || Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                if (position.X <= rBound)
                    position.X += 7;
            }

        }

        /// 
        /// Draws the sprite on the screen.
        /// 
        /// <param name="sb"></param>
        public void Draw(SpriteBatch sb)
        {
            sb.Draw(texture, new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height), Color.White);
        }

        /// 
        /// Checks if the paddle collides with the ball.
        /// 
        /// <param name="ball">The ball.</param>
        /// <returns>True in case of collision and False otherwise.</returns>
        public bool CollidesWith(Ball ball)
        {
            var box = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
            var bb = new Rectangle((int)(ball.Position.X - ball.Radius), (int)(ball.Position.Y - ball.Radius), (int)ball.Radius * 2, (int)ball.Radius * 2);
            if (box.Intersects(bb))
            {
                var interX = MathHelper.Clamp(ball.Position.X, position.X, position.X + texture.Width);
                var interY = MathHelper.Clamp(ball.Position.Y, position.Y, position.Y + texture.Height);
                var closest = new Vector2(interX, interY);
                if (Vector2.Distance(closest, ball.Position) < ball.Radius)
                {
                    return true;
                }
            }
            return false;
        }

        /// 
        /// Checks if the paddle colllides with the power up.
        /// 
        /// <param name="pu">The Power up</param>
        /// <returns>True in case of collision. False otherwise.</returns>
        public bool CollidesWith(PowerUp pu)
        {
            var box = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
            var bb = pu.Bounds;
            if (box.Intersects(bb))
            {
                return true;
            }
            return false;
        }

        /// 
        /// The position of the paddle.
        /// 
        public Vector2 Center
        {
            get
            {
                return new Vector2(position.X + texture.Width / 2, position.Y + texture.Height / 2);
            }
        }

        /// 
        /// Deflects the ball by changing its movement direction.
        /// 
        /// <param name="ball">The ball.</param>
        public void HandleCollision(Ball ball)
        {
            Vector2 direction = Vector2.Normalize(ball.Position - Center);
            ball.Velocity = direction * 2.0f;
        }
    }
}
