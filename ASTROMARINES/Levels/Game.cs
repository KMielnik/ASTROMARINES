using ASTROMARINES.Characters.Player;
using ASTROMARINES.Other;
using ASTROMARINES.Properties;
using SFML.Audio;
using SFML.Graphics;
using SFML.Window;
using System;
using System.Collections.Generic;

namespace ASTROMARINES.Levels
{
    class Game : IDisposable
    {
        IPlayer player;
        Queue<Tuple<string,string>> levelNamesQueue;
        ILevel currentLevel;
        Menu menu;
        RenderWindow window;
        Music mainMenuMusic;

        public Game(RenderWindow window)
        {
            WindowProperties.WindowWidth = window.Size.X;
            WindowProperties.WindowHeight = window.Size.Y;
            window.SetFramerateLimit(60);
            window.SetMouseCursorVisible(false);
            window.SetVerticalSyncEnabled(true);

            player = new Player();
            currentLevel = new SimpleImageScreen(Resources.TitleBG);
            levelNamesQueue = new Queue<Tuple<string, string>>();
            levelNamesQueue.Enqueue(new Tuple<string, string>("SimpleImageScreen", Resources.PlotBG));
            menu = new Menu();
            this.window = window;

            mainMenuMusic = new Music(Resources.MenuBGMusic);
            mainMenuMusic.Loop = true;
            mainMenuMusic.Play();
        }

        public void Run()
        {
            if(currentLevel.HasLevelEnded)
            {
                if (levelNamesQueue.Count == 0)
                {
                    if (mainMenuMusic.Status == SoundStatus.Stopped)
                        mainMenuMusic.Play();
                    levelNamesQueue = menu.MenuLogic(window);
                    menu.Draw(window);
                    if (levelNamesQueue.Count != 0)
                    {
                        player.Dispose();
                        player = new Player();
                    }
                }
                else
                {
                    currentLevel.Dispose();
                    var levelNameAndArg = levelNamesQueue.Dequeue();
                    var levelType = Type.GetType($"ASTROMARINES.Levels.{levelNameAndArg.Item1}");

                    if (levelNameAndArg.Item2.Equals("SendPlayerAsArgument"))
                    {
                        currentLevel = (ILevel)Activator.CreateInstance(levelType, player);
                        if (mainMenuMusic.Status == SoundStatus.Playing)
                            mainMenuMusic.Stop();
                    }
                    else
                    {
                        currentLevel = (ILevel)Activator.CreateInstance(levelType, levelNameAndArg.Item2);
                        if (mainMenuMusic.Status == SoundStatus.Stopped)
                            mainMenuMusic.Play();
                    }
                }
            }
            else
            {
                currentLevel.LevelLogic(window);
                currentLevel.Draw(window);
            }
            
            if(player.ShouldBeDeleted)
            {
                currentLevel.Dispose();
                currentLevel = new SimpleTextScreen("YOU DIED");
                mainMenuMusic.Play();
                player.Dispose();
                player = new Player();
                levelNamesQueue.Clear();
            }
        }

        public void Dispose()
        {
            player.Dispose();
            currentLevel.Dispose();
            menu.Dispose();
            mainMenuMusic.Dispose();
        }
    }
}