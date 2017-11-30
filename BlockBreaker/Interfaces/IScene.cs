using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace EECEBlockBreaker.Interfaces
{
    interface IScene : IGameObject
    {
        bool Completed
        {
            get;
        }
    }
}
