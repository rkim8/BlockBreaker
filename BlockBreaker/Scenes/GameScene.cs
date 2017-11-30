using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

using EECEBlockBreaker.Interfaces;

namespace EECEBlockBreaker.Scenes 
{
    class GameScene : IScene
    {
        /// 
        /// If the scene has completed and is ready to be swapped out.
        /// 
        public bool Completed
        {
            get;
            private set;
        }

        Level level;
        Player player;
        Paddle paddle = new Paddle();
        List<Ball> balls = new List<Ball>();
        List<PowerUp> PowerUps = new List<PowerUp>();

        Random random = new Random();

        bool paused = true;
        double pauseTime = 0.0f;

        double timeLeft = 4 * 60;

        ContentManager content;
        SpriteFont font;
        SpriteFont hsfont;
        SoundEffect blockDest, blockHit, pUpBigger, pDownSmaller, pDownFast, pUpExtraBall, pUpExtraLife;

        /// 
        /// Constructor for the GameScene class. 
        /// 
        /// <param name="p">Current player</param>
        /// <param name="l">Level</param>
        public GameScene(Player p, Level l)
        {
            player = p;
            level = l;
            balls.Add(new Ball(200, 340));
        }


        /// 
        /// Loads the sprites and sound effects from the project content.
        /// 
        /// <param name="c">Project Content</param>
        public void Load(ContentManager c)
        {
            content = c;
            paddle.Load(c);
            level.Load(c);
            level.OnBlockCollision += OnCollision;
            font = c.Load<SpriteFont>("font");
            hsfont = c.Load<SpriteFont>("hsfont");
            blockDest = c.Load<SoundEffect>("BlockDestroyed");
            pUpBigger = c.Load<SoundEffect>("Bigger");
            pDownSmaller = c.Load<SoundEffect>("Smaller");
            pDownFast = c.Load<SoundEffect>("Fast");
            blockHit = c.Load<SoundEffect>("BlockHit");
            pUpExtraBall = c.Load<SoundEffect>("ExtBall");
            pUpExtraLife = c.Load<SoundEffect>("ExtLife");
            foreach (Ball b in balls)
            {
                b.Load(c);
            }
            foreach (PowerUp PU in PowerUps)
            {
                PU.Load(c);
            }
        }

        /// 
        /// Plays a sound effect upon the collision of a ball with a block.
        /// Plays a sound effect and randomly spawns power ups when a block is destroyed.
        /// 
        /// <param name="e"></param>
        /// <param name="cea">Position of the block hit</param>
        void OnCollision(object e, Level.CollisionEventArgs cea)
        {

            if (cea.Block.Destroyed)
            {
                player.AddToScore(50);
                blockDest.Play(0.5f, 0, 0);
                // 10% chance for a powerup to be dropped.
                if (random.Next(1, 100) <= 15)
                {
                    int type = random.Next(1, 100);
                    PowerUp.PowerUpType kind;

                    // Pick which type.
                    if (type < 20)
                    {
                        kind = PowerUp.PowerUpType.BigPaddle;
                    }
                    else if (type < 40)
                    {
                        kind = PowerUp.PowerUpType.SmallPaddle;
                    }
                    else if (type < 60)
                    {
                        kind = PowerUp.PowerUpType.FastBall;
                    }
                    else if (type < 90)
                    {
                        kind = PowerUp.PowerUpType.ExtraBall;
                    }
                    else
                    {
                        kind = PowerUp.PowerUpType.PlusLife;
                    }

                    PowerUp p = new PowerUp(cea.Block.Center, kind);
                    p.Load(content);
                    PowerUps.Add(p);
                }
            }
            else
            {
                blockDest.Play(0.02f, 0, 0);
            }
        }

        /// 
        /// Updates the game scene. Preforms all time based actions.
        /// 
        /// <param name="gameTime">The elapsed time since the last frame.</param>
        public void Update(GameTime gameTime)
        {
            if (!paused)
            {
                timeLeft -= gameTime.ElapsedGameTime.TotalSeconds;

                // Update the entities.
                paddle.Update(gameTime);
                level.Update(gameTime);
                List<Ball> toRemove = new List<Ball>();
                List<PowerUp> toRemovePU = new List<PowerUp>();

                // Check if we have hit any power ups, and apply them.
                foreach (PowerUp p in PowerUps)
                {
                    p.Update(gameTime);

                    if (paddle.CollidesWith(p))
                    {
                        toRemovePU.Add(p);
                        switch (p.Type)
                        {
                            case PowerUp.PowerUpType.BigPaddle:
                                pUpBigger.Play();
                                paddle.MakeBigger();
                                break;

                            case PowerUp.PowerUpType.SmallPaddle:
                                pDownSmaller.Play();
                                paddle.MakeSmaller();
                                break;

                            case PowerUp.PowerUpType.FastBall:
                                pDownFast.Play();
                                foreach (Ball b in balls)
                                {
                                    b.FasterBall();
                                }
                                break;

                            case PowerUp.PowerUpType.ExtraBall:
                                pUpExtraBall.Play();
                                Ball newball = new Ball(200, 340);
                                balls.Add(newball);
                                newball.Load(content);
                                break;

                            case PowerUp.PowerUpType.PlusLife:
                                pUpExtraLife.Play();
                                player.Lives++;
                                break;
                        }
                    }
                }

                // Handle ball collisions, and removing balls that have left the level.
                foreach (Ball ball in balls)
                {
                    ball.Update(gameTime);

                    if (level.CollidesWith(ball))
                    {
                        level.HandleCollision(ball);
                    }
                    if (paddle.CollidesWith(ball))
                    {
                        paddle.HandleCollision(ball);
                    }

                    if (ball.Position.Y < 0 || ball.Position.Y > 415 || ball.Position.X < 0 || ball.Position.X > 460)
                    {
                        toRemove.Add(ball);
                        player.Lives--;
                        if (player.Lives == 0)
                        {
                            player.AddToScore(timeLeft);
                            Completed = true;
                        }
                    }
                }
           
                // Check if all the blocks are removed.
                if (level.IsFinished())
                {
                    player.AddToScore(timeLeft);
                    Completed = true;
                }
                // Check if we ran out of time.
                if (timeLeft <= 0)
                {
                    player.Lives = 0;
                    Completed = true;
                }

                // Update our lists.
                PowerUps.RemoveAll(PU => toRemovePU.Contains(PU));
                balls.RemoveAll(ball => toRemove.Contains(ball));

                // Spawn a new ball if none are left.
                if (balls.Count == 0)
                {
                    Ball ball = new Ball(200, 340);
                    ball.Load(content);
                    balls.Add(ball);
                }
            }

            // Allow pausing, with a 0.5s delay to stop spamming.
            pauseTime += gameTime.ElapsedGameTime.TotalSeconds;
            if (Keyboard.GetState().IsKeyDown(Keys.Space) && pauseTime > 0.5)
            {
                pauseTime = 0;
                paused = !paused;
            }
        }

        /// 
        /// Draws the scene to the spritebatch.
        /// 
        /// <param name="spriteBatch">The target spritebatch.</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            // Draw entities.
            paddle.Draw(spriteBatch);
            level.Draw(spriteBatch);
            foreach (Ball b in balls)
            {
                b.Draw(spriteBatch);
            }
            foreach (PowerUp PU in PowerUps)
            {
                PU.Draw(spriteBatch);
            }

            // Format and draw the display.
            string time = String.Format("Time: {0}:{1:D2}", (int)Math.Floor(timeLeft/60), (int)timeLeft % 60);
            spriteBatch.DrawString(font, time, new Vector2(5, 4), (timeLeft < 60 && (int)timeLeft % 2 == 0) ? Color.DarkCyan : Color.LightBlue);

            string score = String.Format("Score: {0:D6}", player.Score);
            spriteBatch.DrawString(font, score, new Vector2(164, 4), Color.WhiteSmoke);

            string lives = String.Format("Lives: {0}", player.Lives);
            spriteBatch.DrawString(font, lives, new Vector2(370, 4), Color.Red);

            if (paused)
            {
                spriteBatch.DrawString(hsfont, "PAUSED", new Vector2(165, 180), Color.Red);
                spriteBatch.DrawString(font, "Press 'Space' to continue...", new Vector2(94, 224), Color.Red);
            }
        }
    }
    
}
