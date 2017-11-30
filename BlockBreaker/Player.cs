using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EECEBlockBreaker
{
    class Player
    {
        /// 
        /// Lives left.
        /// 
        public int Lives
        {
            get;
            set;
        }

        /// 
        /// Current score.
        /// 
        public int Score
        {
            get;
            private set;
        }

        public Player()
        {
            Score = 0;
            Lives = 5;
        }

        /// 
        /// Add a value to the score.
        /// 
        /// <param name="seconds"></param>
        public void AddToScore(double seconds)
        {
            Score += (int)Math.Floor(seconds);
        }
    }
}
