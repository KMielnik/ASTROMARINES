using System;
using System.Collections.Generic;
using ASTROMARINES.Other;
using ASTROMARINES.Properties;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace ASTROMARINES.Levels
{
    class Menu : IDisposable
    {
        private MousePointer _mousePointer;
        private readonly Texture _backgroundTexture;
        private readonly Sprite _background;
        private readonly List<Button> _buttons;
        
        public Menu()
        {
            _mousePointer = new MousePointer();
            _backgroundTexture = new Texture(Resources.MenuBG);
            _background = new Sprite(_backgroundTexture);
            _background.Scale = new Vector2f(WindowProperties.ScaleX, WindowProperties.ScaleY);
            _buttons = new List<Button>
            {
                new Button("START",         new Vector2f(WindowProperties.WindowWidth * 0.3f, WindowProperties.WindowHeight * 20 / 50f)),
                new Button("HOW TO PLAY",   new Vector2f(WindowProperties.WindowWidth * 0.3f, WindowProperties.WindowHeight * 26 / 50f)),
                new Button("GRAPHICS",      new Vector2f(WindowProperties.WindowWidth * 0.3f, WindowProperties.WindowHeight * 32 / 50f)),
                new Button("CREDITS",       new Vector2f(WindowProperties.WindowWidth * 0.3f, WindowProperties.WindowHeight * 38 / 50f)),
                new Button("EXIT",          new Vector2f(WindowProperties.WindowWidth * 0.3f, WindowProperties.WindowHeight * 44 / 50f))
            };
        }

        /// <summary>
        /// Performs menu logic, and tries to return choosen
        /// levelQueue, if not choosen returns empty queue to give another frame in menu
        /// </summary>
        /// <returns></returns>
        public Queue<(string name, string arg)> MenuLogic(RenderWindow window)
        {
            var levelNamesQueue = new Queue<(string name, string arg)>();

            var mousePosition = Mouse.GetPosition(window);

            _mousePointer.HoversOverItemOff();
            foreach (var button in _buttons)
            {
                if (button.BoundingBox.Contains(mousePosition.X, mousePosition.Y))
                {
                    _mousePointer.HoversOverItemOn();
                    if (Mouse.IsButtonPressed(Mouse.Button.Left))
                        switch (button.Label)
                        {
                            case "START":
                                levelNamesQueue.Enqueue((name: "SimpleTextScreen", arg: "your journey begins"));
                                levelNamesQueue.Enqueue((name: "SimpleTextScreen", arg: "estimated travel time to the next level: 60 seconds"));
                                levelNamesQueue.Enqueue((name: "Level1"          , arg: "SendPlayerAsArgument"));
                                levelNamesQueue.Enqueue((name: "SimpleTextScreen", arg: "INCOMING!"));
                                levelNamesQueue.Enqueue((name: "SimpleTextScreen", arg: "from EVERY SIDE!"));
                                levelNamesQueue.Enqueue((name: "Level2"          , arg: "SendPlayerAsArgument"));
                                levelNamesQueue.Enqueue((name: "SimpleTextScreen", arg: "thanks to all those powerups"));
                                levelNamesQueue.Enqueue((name: "SimpleTextScreen", arg: "you have EVOLVED"));
                                levelNamesQueue.Enqueue((name: "SimpleTextScreen", arg: "you will need those new powers"));
                                levelNamesQueue.Enqueue((name: "SimpleTextScreen", arg: "RIGHT NOW"));
                                levelNamesQueue.Enqueue((name: "Level3"          , arg: "SendPlayerAsArgument"));
                                levelNamesQueue.Enqueue((name: "SimpleTextScreen", arg: "you found him"));
                                levelNamesQueue.Enqueue((name: "SimpleTextScreen", arg: "your final enemy"));
                                levelNamesQueue.Enqueue((name: "SimpleTextScreen", arg: "THE TRASH BOSS"));
                                levelNamesQueue.Enqueue((name: "LevelBoss"       , arg: "SendPlayerAsArgument"));
                                levelNamesQueue.Enqueue((name: "SimpleTextScreen", arg: "you did it!"));
                                levelNamesQueue.Enqueue((name: "SimpleTextScreen", arg: "you saved THE UNIVERSE!"));
                                levelNamesQueue.Enqueue((name: "SimpleTextScreen", arg: "Congratulations!"));
                                break;

                            case "HOW TO PLAY":
                                levelNamesQueue.Enqueue((name: "SimpleImageScreen",arg:  Resources.HowToPlayBG));
                                break;
                            case "GRAPHICS":
                                levelNamesQueue.Enqueue((name: "GraphicsSettings" ,arg: ""));
                                break;

                            case "CREDITS":
                                levelNamesQueue.Enqueue((name: "SimpleImageScreen",arg:  Resources.CreditsBG));
                                break;

                            case "EXIT":
                                window.Close();
                                break;
                        }
                }
            }
            return levelNamesQueue;
        }

        public void ResetToNewResolution()
        {
            _background.Scale = new Vector2f(WindowProperties.ScaleX, WindowProperties.ScaleY);
            for(var i=0;i<_buttons.Count;i++)
                _buttons[i].SetPosition(new Vector2f(WindowProperties.WindowWidth * 0.3f, WindowProperties.WindowHeight * (20 + 6 * i) / 50f));
            _mousePointer = new MousePointer();
        }

        public void Draw(RenderWindow window)
        {
            window.Clear(Color.Black);
            window.Draw(_background);
            foreach (var button in _buttons)
                button.Draw(window);
            _mousePointer.Draw(window);

            window.Display();
        }

        public void Dispose()
        {
            _mousePointer.Dispose();
            _backgroundTexture.Dispose();
            _background.Dispose();
            foreach (var button in _buttons)
                button.Dispose();
        }
    }
}