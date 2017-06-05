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
        private MousePointer mousePointer;
        private Texture backgroundTexture;
        private Sprite background;
        private List<Button> buttons;
        
        public Menu()
        {
            mousePointer = new MousePointer();
            backgroundTexture = new Texture(Resources.MenuBG);
            background = new Sprite(backgroundTexture);
            background.Scale = new Vector2f(WindowProperties.ScaleX, WindowProperties.ScaleY);
            buttons = new List<Button>
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

            mousePointer.HoversOverItemOff();
            foreach (var button in buttons)
            {
                if (button.BoundingBox.Contains(mousePosition.X, mousePosition.Y))
                {
                    mousePointer.HoversOverItemOn();
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
            background.Scale = new Vector2f(WindowProperties.ScaleX, WindowProperties.ScaleY);
            for(int i=0;i<buttons.Count;i++)
                buttons[i].SetPosition(new Vector2f(WindowProperties.WindowWidth * 0.3f, WindowProperties.WindowHeight * (20 + 6 * i) / 50f));
            mousePointer = new MousePointer();
        }

        public void Draw(RenderWindow window)
        {
            window.Clear(Color.Black);
            window.Draw(background);
            foreach (var button in buttons)
                button.Draw(window);
            mousePointer.Draw(window);

            window.Display();
        }

        public void Dispose()
        {
            mousePointer.Dispose();
            backgroundTexture.Dispose();
            background.Dispose();
            foreach (var button in buttons)
                button.Dispose();
        }
    }
}