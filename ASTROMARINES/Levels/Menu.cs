using ASTROMARINES.Other;
using ASTROMARINES.Properties;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;

namespace ASTROMARINES.Levels
{
    partial class Menu : IDisposable
    {
        MousePointer mousePointer;
        Texture backgroundTexture;
        Sprite background;
        List<Button> buttons;
        
        public Menu()
        {
            mousePointer = new MousePointer();
            backgroundTexture = new Texture(Resources.MenuBG);
            background = new Sprite(backgroundTexture);
            background.Scale = new Vector2f(WindowProperties.ScaleX, WindowProperties.ScaleY);
            buttons = new List<Button>();

            buttons.Add(new Button("START",       new Vector2f(WindowProperties.WindowWidth / 1.3f, WindowProperties.WindowHeight * 28 / 50f)));
            buttons.Add(new Button("HOW TO PLAY", new Vector2f(WindowProperties.WindowWidth / 1.3f, WindowProperties.WindowHeight * 34 / 50f)));
            buttons.Add(new Button("EXIT",        new Vector2f(WindowProperties.WindowWidth / 1.3f, WindowProperties.WindowHeight * 40 / 50f)));
        }

        /// <summary>
        /// Performs menu logic, and tries to return choosen
        /// levelQueue, if not choosen returns empty queue to give another frame in menu
        /// </summary>
        /// <returns></returns>
        public Queue<Tuple<string, string>> MenuLogic(RenderWindow window)
        {
            var levelNamesQueue = new Queue<Tuple<string, string>>();

            var mousePosition = Mouse.GetPosition(window);

            mousePointer.HoversOverItemOFF();
            foreach (var button in buttons)
            {
                if (button.BoundingBox.Contains(mousePosition.X, mousePosition.Y))
                {
                    mousePointer.HoversOverItemON();
                    if (Mouse.IsButtonPressed(Mouse.Button.Left))
                        switch (button.Label)
                        {
                            case "START":
                                levelNamesQueue.Enqueue(new Tuple<string, string>("SimpleTextScreen", "Your journey begins"));
                                levelNamesQueue.Enqueue(new Tuple<string, string>("Level1", "SendPlayerAsArgument"));
                                levelNamesQueue.Enqueue(new Tuple<string, string>("SimpleTextScreen", "INCOMING"));
                                levelNamesQueue.Enqueue(new Tuple<string, string>("Level2", "SendPlayerAsArgument"));
                                break;

                            case "HOW TO PLAY":
                                levelNamesQueue.Enqueue(new Tuple<string, string>("SimpleImageScreen", Resources.HowToPlayBG));
                                break;

                            case "EXIT":
                                window.Close();
                                break;
                        }
                }
            }
            return levelNamesQueue;
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