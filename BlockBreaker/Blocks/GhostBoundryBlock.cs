using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace EECEBlockBreaker
{
    class GhostBoundryBlock : Block
    {
        /// 
        /// Block constructer.
        /// 
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        public GhostBoundryBlock(int x, int y)
            : base(x, y)
        {
            color = Color.DarkGray;
        }

        float alpha = 1.0f;

        /// 
        /// Checks if the block is a natural part of the level (isn't required to be removed for victory).
        /// 
        public override bool Static
        {
            get
            {
                return true;
            }
        }

        /// 
        /// Checks if the ball has collided with the block.
        /// 
        /// <param name="ball">The ball.</param>
        /// <returns></returns>
        public override bool CollidesWith(Ball ball)
        {
            // Can only collide if we are 'visible'.
            if (alpha > 0.8)
            {
                return base.CollidesWith(ball);
            }
            return false;
        }

        /// 
        /// Handles collision with a ball. Boundry blocks cannot be destroyed.
        /// 
        /// <param name="ball">The ball.</param>
        public override void HandleCollision(Ball ball)
        {
            base.HandleCollision(ball);
            Destroyed = false;
        }

        /// 
        /// Updates the blocks state for dynamic blocks.
        /// 
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            // Change transparency based on time.
            alpha = (float)Math.Abs(Math.Sin(gameTime.TotalGameTime.TotalSeconds / 3.0));
            color = Color.Lerp(Color.Transparent, Color.DarkGray, alpha);
            base.Update(gameTime);
        }
    }
}
