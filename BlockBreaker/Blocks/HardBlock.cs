using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace EECEBlockBreaker
{
    class HardBlock : Block
    {
        /// 
        /// Block constructer.
        /// 
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        public HardBlock(int x, int y) : base(x, y) 
        {
            color = Color.DarkGreen;
        }

        bool hitOnce = false;

        /// 
        /// Handles collision with a ball. Hard blocks take two hits to be destroyed.
        /// 
        /// <param name="ball">The ball.</param>
        public override void HandleCollision(Ball ball)
        {
            base.HandleCollision(ball);
            if (!hitOnce)
            {
                color = Color.LightGreen;
                hitOnce = true;
                Destroyed = false;
            }
        }
    }
}
