using System;
using System.Collections.Generic;
using ASTROMARINES.Characters.Player;
using ASTROMARINES.Other;
using ASTROMARINES.Properties;
using SFML.Audio;
using SFML.Graphics;
using SFML.Window;

namespace ASTROMARINES.Levels
{
    internal class Game : IDisposable
    {
        private IPlayer player;
        private Queue<(string name, string arg)> levelNamesQueue;
        private ILevel currentLevel;
        private Menu menu;
        private RenderWindow window;
        private Music mainMenuMusic;

        public Game()
        {
            window = new RenderWindow(VideoMode.DesktopMode, "ASTROMARINES", Styles.Fullscreen);
            window.KeyPressed += Window_KeyPressed;
            window.Closed += (s, a) => window.Close();
            window.SetFramerateLimit(60);
            window.SetMouseCursorVisible(false);
            window.SetVerticalSyncEnabled(true);

            WindowProperties.WindowWidth = window.Size.X;
            WindowProperties.WindowHeight = window.Size.Y;

            player = new Player();
            currentLevel = new SimpleImageScreen(Resources.TitleBG);
            levelNamesQueue = new Queue<(string name, string arg)>();
            levelNamesQueue.Enqueue((name: "SimpleImageScreen", arg: Resources.PlotBG));
            menu = new Menu();

            mainMenuMusic = new Music(Resources.MenuBGMusic);
            mainMenuMusic.Loop = true;
            mainMenuMusic.Play();
        }

        public void Run()
        {
            while (window.IsOpen)
            {
                window.DispatchEvents();
                NextFrame();
            }
        }
        private void NextFrame()
        {
            if (currentLevel.HasLevelEnded)
            {
                menu.ResetToNewResolution();
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
                    var (levelName,levelArg) = levelNamesQueue.Dequeue();
                    var levelType = Type.GetType($"ASTROMARINES.Levels.{levelName}");

                    if (levelArg.Equals("SendPlayerAsArgument"))
                    {
                        if (levelType != null)
                            currentLevel = (ILevel)Activator.CreateInstance(levelType, player);
                        if (mainMenuMusic.Status == SoundStatus.Playing)
                            mainMenuMusic.Stop();
                    }
                    else
                    {
                        if (levelType != null)
                            currentLevel = (ILevel)Activator.CreateInstance(levelType, levelArg);
                        if (mainMenuMusic.Status == SoundStatus.Stopped)
                            mainMenuMusic.Play();
                    }
                }
            }
            else
            {
                currentLevel.LevelLogic(ref window);
                currentLevel.Draw(window);
            }

            if (player.ShouldBeDeleted)
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

        private void Window_KeyPressed(object sender, KeyEventArgs e)
        {
            if (e.Code == Keyboard.Key.Escape)
            {
                window.Close();
            }
        }
    }
}