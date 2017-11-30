using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace EECEBlockBreaker.Interfaces
{
    interface IGameObject
    {
        void Load(ContentManager content);
        void Update(GameTime gameTime);
        void Draw(SpriteBatch sb);
    }
}
