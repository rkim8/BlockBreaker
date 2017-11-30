using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EECEBlockBreaker.Interfaces
{
    interface ICollidable
    {
        bool CollidesWith(Ball ball);
        void HandleCollision(Ball ball);
    }
}
