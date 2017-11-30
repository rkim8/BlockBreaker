using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace EECEBlockBreaker
{
    class BoundryBlock : Block
    {
        /// 
        /// Block constructer.
        /// 
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        public BoundryBlock(int x, int y) : base(x, y) 
        {
            color = Color.DarkGray;
        }

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
        /// Handles collision with a ball. Boundry blocks cannot be destroyed.
        /// 
        /// <param name="ball">The ball.</param>
        public override void HandleCollision(Ball ball)
        {
            base.HandleCollision(ball);
            Destroyed = false;
        }
    }
}
