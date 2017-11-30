using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using EECEBlockBreaker.Interfaces;

namespace EECEBlockBreaker.Scenes
{
    class HighScoreScene : IScene
    {
        class Highscore
        {
            public string Name { get; set; }
            public int Score { get; set; }

            public Highscore(string name, int score)
            {
                Name = name;
                Score = score;
            }
        }

        static readonly string path = "highscores.txt";

        Player player;
        List<Highscore> scores = new List<Highscore>();
        SpriteFont font;
        bool newHighScore = false;

        /// 
        /// Constructer.
        /// 
        /// <param name="p">The player whos score to compare and display.</param>
        public HighScoreScene(Player p) 
        {
            player = p;
            Completed = false;
        }

        /// 
        /// If the scene has completed and is ready to be swapped out.
        /// 
        public bool Completed
        {
            get;
            private set;
        }

        /// 
        /// Loads the font and the highscores list required for the scene.
        /// 
        /// <param name="content">The content manager.</param>
        public void Load(ContentManager content)
        {
            font = content.Load<SpriteFont>("hsfont");

            bool loadDefaults = false;
            // Check if we have a highscore file.
            if (File.Exists(path))
            {
                // If we do, parse it.
                using (StreamReader file = File.OpenText(path))
                {
                    for (int i = 0; i < 5; ++i)
                    {
                        string line = file.ReadLine();
                        if (line == null)
                        {
                            loadDefaults = true;
                            break;
                        }

                        string[] tok = line.Split(' ');
                        if (tok.Length != 2)
                        {
                            loadDefaults = true;
                            break;
                        }

                        int score = 0;
                        if (!int.TryParse(tok[1], out score))
                        {
                            loadDefaults = true;
                            break;
                        }

                        scores.Add(new Highscore(tok[0], score));
                    }
                }
            }
            else
            {
                loadDefaults = true;
            }

            if (loadDefaults)
            {
                // We don't have a highscores list, or there was a parse error. Use defaults.
                scores.Clear();
                scores.Add(new Highscore("PRH", 500));
                scores.Add(new Highscore("SAK", 400));
                scores.Add(new Highscore("LOL", 300));
                scores.Add(new Highscore("SCR", 200));
                scores.Add(new Highscore("LOW", 100));
            }

            // Check if we have a new highscore.
            foreach (Highscore hs in scores)
            {
                if (player.Score > hs.Score)
                {
                    newHighScore = true;
                    break;
                }
            }
            
        }

        KeyboardState keyState = Keyboard.GetState();
        string name = "";

        public void Update(GameTime gameTime)
        {
            // If we have a new highscore, we should get the name.
            if (newHighScore)
            {
                KeyboardState oldKeyState = keyState;
                keyState = Keyboard.GetState();

                // Get keyboard input in order to get the name.
                foreach (Keys key in keyState.GetPressedKeys())
                {
                    if (oldKeyState.IsKeyUp(key))
                    {
                        if (key == Keys.Back)
                        {
                            // Delete.
                            if (name.Length > 0)
                            {
                                name = name.Remove(name.Length - 1, 1);
                            }
                        }
                        else if (key == Keys.Enter)
                        {
                            // Submit. Update the highscores file and list.
                            newHighScore = false;
                            for (int i = 0; i < scores.Count; ++i)
                            {
                                if (scores[i].Score < player.Score)
                                {
                                    scores.Insert(i, new Highscore(name, player.Score));
                                    break;
                                }
                            }

                            using (StreamWriter file = File.CreateText(path))
                            {
                                int lim = 0;
                                foreach (Highscore h in scores)
                                {
                                    if (lim < 5)
                                    {
                                        string score = h.Name + " " + h.Score;
                                        file.WriteLine(score);
                                    }
                                    else
                                    {
                                        break;
                                    }
                                    ++lim;
                                }
                                file.Flush();
                            }
                        }
                        else if (name.Length < 3)
                        {
                            // If there is room left, try and add a character.
                            string k = key.ToString();
                            if (k.Length == 1)
                            {
                                name += key.ToString();
                            }
                        }
                    }
                }
            }
            else
            {
                // Move to a new game.
                if (Keyboard.GetState().IsKeyDown(Keys.Space))
                {
                    Completed = true;
                }
            }
        }

        /// 
        /// Draws the scene to the sprite batch.
        /// 
        /// <param name="sb"></param>
        public void Draw(SpriteBatch sb)
        {
            if (newHighScore)
            {
                // If we have a new highscore, prompt for a name.
                sb.DrawString(font, "New High Score!", new Vector2(70, 135), Color.Red);
                sb.DrawString(font, "Name: " + name, new Vector2(115, 200), Color.WhiteSmoke);
            }
            else
            {
                // Display the highscores.
                sb.DrawString(font, "High Scores", new Vector2(110, 70), Color.Red);
                for (int i = 0; i < 5 && i < scores.Count; ++i)
                {
                    string score = String.Format("{0,-4}{1,6:D6}", scores[i].Name, scores[i].Score);
                    sb.DrawString(font, score, new Vector2(122, 120 + 40 * i), Color.WhiteSmoke);
                }
            }
        }
    }
}
