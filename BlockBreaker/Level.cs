using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace EECEBlockBreaker
{
    class Level : Interfaces.IGameObject, Interfaces.ICollidable
    {
        List<Block> blocks = new List<Block>();

        /// 
        /// Create a new level from a file.
        /// 
        /// <param name="fileName">The file path.</param>
        /// <returns></returns>
        public static Level FromFile(string fileName) 
        {
            // Check if the file exists.
            if (File.Exists(fileName))
            {
                // Open the file and read each line into an array.
                string[] lev = new string[15];
                using (StreamReader sr = File.OpenText(fileName))
                {
                    for (int i = 0; i < 15; ++i)
                    {
                        string s = sr.ReadLine();
                        // If there isn't 15 lines that are 10 characters long (the format) then return null.
                        if (s == null || s.Length != 10)
                        {
                            return null;
                        }
                        lev[i] = s;
                    }
                }

                Level level = new Level();
                for (int x = 0; x < 15; ++x)
                {
                    for (int y = 0; y < 10; ++y)
                    {
                        // Add a block based on the character.
                        char bt = lev[x][y];
                        switch(bt) {
                            case 'X':
                                level.blocks.Add(new BoundryBlock(y * 46, x * 24 + 31));
                                break;
                            case 'h':
                                level.blocks.Add(new HardBlock(y * 46, x * 24 + 31));
                                break;
                            case 'g':
                                level.blocks.Add(new GhostBlock(y * 46, x * 24 + 31));
                                break;
                            case 'G':
                                level.blocks.Add(new GhostBoundryBlock(y * 46, x * 24 + 31));
                                break;
                            case 'n':
                                level.blocks.Add(new Block(y * 46, x * 24 + 31));
                                break;
                            default:
                                // Air.
                                break;
                        }
                    }
                }
                return level;
            }
            return null;
        }

        // Only allow FromFile creation.
        private Level() {}

        /// 
        /// Checks if the level is completed.
        /// 
        /// <returns></returns>
        public bool IsFinished()
        {
            foreach (Block b in blocks)
            {
                if (!b.Destroyed && !b.Static)
                {
                    return false;
                }
            }
            return true;
        }

        /// 
        /// Checks if any of the balls collide with ball.
        /// 
        /// <param name="ball">The ball.</param>
        /// <returns></returns>
        public bool CollidesWith(Ball ball)
        {
            foreach (Block b in blocks)
            {
                if (b.CollidesWith(ball))
                {
                    return true;
                }
            }
            return false;
        }

        /// 
        /// Handles all collisions between the blocks and the ball.
        /// 
        /// <param name="ball">The ball.</param>
        public void HandleCollision(Ball ball)
        {
            foreach (Block b in blocks)
            {
                if (b.CollidesWith(ball))
                {
                    b.HandleCollision(ball);
                    if (OnBlockCollision != null)
                    {
                        OnBlockCollision(this, new CollisionEventArgs(b));
                    }
                }
            }
        }

        public class CollisionEventArgs : EventArgs {
            public Block Block {
                get;
                private set;
            }

            public CollisionEventArgs(Block b) {
                Block = b;
            }
        }

        /// 
        /// This event is raised when a block is hit.
        /// 
        public event EventHandler<CollisionEventArgs> OnBlockCollision;

        /// 
        /// Loads all the blocks.
        /// 
        /// <param name="content">The content manager.</param>
        public void Load(Microsoft.Xna.Framework.Content.ContentManager content)
        {
            foreach (Block b in blocks)
            {
                b.Load(content);
            }
        }

        /// 
        /// Updates all the blocks in the level.
        /// 
        /// <param name="gameTime"></param>
        public void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            foreach (Block b in blocks)
            {
                b.Update(gameTime);
            }
        }

        /// 
        /// Draws all the blocks to the sprite batch.
        /// 
        /// <param name="sb"></param>
        public void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch sb)
        {
            foreach (Block b in blocks)
            {
                b.Draw(sb);
            }
        }
    }
}
