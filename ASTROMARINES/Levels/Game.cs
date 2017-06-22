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
        private IPlayer _player;
        private Queue<(string name, string arg)> _levelNamesQueue;
        private ILevel _currentLevel;
        private readonly Menu _menu;
        private RenderWindow _window;
        private readonly Music _mainMenuMusic;

        public Game()
        {
            _window = new RenderWindow(VideoMode.DesktopMode, "ASTROMARINES", Styles.Fullscreen);
            _window.KeyPressed += Window_KeyPressed;
            _window.Closed += (s, a) => _window.Close();
            _window.SetFramerateLimit(60);
            _window.SetMouseCursorVisible(false);
            _window.SetVerticalSyncEnabled(true);

            WindowProperties.WindowWidth = _window.Size.X;
            WindowProperties.WindowHeight = _window.Size.Y;

            _player = new Player();
            _currentLevel = new SimpleImageScreen(Resources.TitleBG);
            _levelNamesQueue = new Queue<(string name, string arg)>();
            _levelNamesQueue.Enqueue((name: "SimpleImageScreen", arg: Resources.PlotBG));
            _menu = new Menu();

            _mainMenuMusic = new Music(Resources.MenuBGMusic);
            _mainMenuMusic.Loop = true;
            _mainMenuMusic.Play();
        }

        public void Run()
        {
            while (_window.IsOpen)
            {
                _window.DispatchEvents();
                NextFrame();
            }
        }
        private void NextFrame()
        {
            if (_currentLevel.HasLevelEnded)
            {
                _menu.ResetToNewResolution();
                if (_levelNamesQueue.Count == 0)
                {
                    if (_mainMenuMusic.Status == SoundStatus.Stopped)
                        _mainMenuMusic.Play();
                    _levelNamesQueue = _menu.MenuLogic(_window);
                    _menu.Draw(_window);
                    if (_levelNamesQueue.Count != 0)
                    {
                        _player.Dispose();
                        _player = new Player();
                    }
                }
                else
                {
                    _currentLevel.Dispose();
                    var (levelName,levelArg) = _levelNamesQueue.Dequeue();
                    var levelType = Type.GetType($"ASTROMARINES.Levels.{levelName}");

                    if (levelArg.Equals("SendPlayerAsArgument"))
                    {
                        if (levelType != null)
                            _currentLevel = (ILevel)Activator.CreateInstance(levelType, _player);
                        if (_mainMenuMusic.Status == SoundStatus.Playing)
                            _mainMenuMusic.Stop();
                    }
                    else
                    {
                        if (levelType != null)
                            _currentLevel = (ILevel)Activator.CreateInstance(levelType, levelArg);
                        if (_mainMenuMusic.Status == SoundStatus.Stopped)
                            _mainMenuMusic.Play();
                    }
                }
            }
            else
            {
                _currentLevel.LevelLogic(ref _window);
                _currentLevel.Draw(_window);
            }

            if (_player.ShouldBeDeleted)
            {
                _currentLevel.Dispose();
                _currentLevel = new SimpleTextScreen("YOU DIED");
                _mainMenuMusic.Play();
                _player.Dispose();
                _player = new Player();
                _levelNamesQueue.Clear();
            }
        }

        public void Dispose()
        {
            _player.Dispose();
            _currentLevel.Dispose();
            _menu.Dispose();
            _mainMenuMusic.Dispose();
        }

        private void Window_KeyPressed(object sender, KeyEventArgs e)
        {
            if (e.Code == Keyboard.Key.Escape)
            {
                _window.Close();
            }
        }
    }
}