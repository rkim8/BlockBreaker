using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace EECEBlockBreaker
{
    class Block : Interfaces.ICollidable, Interfaces.IGameObject
    {
        protected Texture2D texture;
        protected Vector2 position;
        protected Color color = Color.LightGreen;

        /// 
        /// If the block is destroyed.
        /// 
        public bool Destroyed {
            get;
            protected set;
        }

        /// 
        /// Checks if the block is a natural part of the level (isn't required to be removed for victory).
        /// 
        public virtual bool Static
        {
            get
            {
                return false;
            }
        }

        /// 
        /// The postion of the block.
        /// 
        public Vector2 Center
        {
            get
            {
                return new Vector2(position.X + texture.Width / 2, position.Y + texture.Height / 2);
            }
        }

        /// 
        /// Block constructer.
        /// 
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        public Block(int x, int y)
        {
            position = new Vector2(x, y);
            Destroyed = false;
        }

        /// 
        /// Checks if the block collides with the ball.
        /// 
        /// <param name="ball">The ball.</param>
        /// <returns></returns>
        public virtual bool CollidesWith(Ball ball)
        {
            if (!Destroyed)
            {
                // Make bounding boxes.
                var box = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
                var bb = new Rectangle((int)(ball.Position.X - ball.Radius), (int)(ball.Position.Y - ball.Radius), (int)ball.Radius * 2, (int)ball.Radius * 2);
                if (box.Intersects(bb))
                {
                    // Determind the closest point on our box to the balls center.
                    var interX = MathHelper.Clamp(ball.Position.X, position.X, position.X + texture.Width);
                    var interY = MathHelper.Clamp(ball.Position.Y, position.Y, position.Y + texture.Height);

                    // Check the distace.
                    var closest = new Vector2(interX, interY);
                    if (Vector2.Distance(closest, ball.Position) < ball.Radius)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// 
        /// Handles collision with a ball.
        /// 
        /// <param name="ball">The ball.</param>
        public virtual void HandleCollision(Ball ball)
        {
            Destroyed = true;

            // Get the direction the ball is relative to us.
            Vector2 direction = Vector2.Normalize(ball.Position - Center);

            // Resolve the collision. Prefer resolving from the sides.
            if (position.X < ball.Position.X && ball.Position.X < position.X + texture.Width)
            {
                direction.X = ball.Velocity.X;
                direction.Y = Math.Sign(direction.Y) * Math.Abs(ball.Velocity.Y);
            }
            else
            {
                direction.Y = ball.Velocity.Y;
                direction.X = Math.Sign(direction.X) * Math.Abs(ball.Velocity.X);
            }
            ball.Velocity = direction;
        }

        /// 
        /// Loads all the required content for the block.
        /// 
        /// <param name="content">The content manager.</param>
        public virtual void Load(Microsoft.Xna.Framework.Content.ContentManager content)
        {
            texture = content.Load<Texture2D>("block");
        }

        /// 
        /// Updates the blocks state for dynamic blocks.
        /// 
        /// <param name="gameTime"></param>
        public virtual void Update(GameTime gameTime) { }


        /// 
        /// Draws the block to the sprite batch.
        /// 
        /// <param name="sb">The sprite batch.</param>
        public virtual void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch sb)
        {
            if (!Destroyed)
            {
                sb.Draw(texture, new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height), color);
            }
        }
    }
}
