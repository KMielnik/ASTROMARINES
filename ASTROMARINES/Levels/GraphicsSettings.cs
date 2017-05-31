using ASTROMARINES.Other;
using ASTROMARINES.Properties;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;

namespace ASTROMARINES.Levels
{
    internal class GraphicsSettings : ILevel
    {
        private Clock clock;
        private MousePointer mousePointer;
        private Texture backgroundTexture;
        private Sprite background;
        private List<Button> buttons;

        public GraphicsSettings(string nonUsed)
        {
            clock = new Clock();
            mousePointer = new MousePointer();
            backgroundTexture = new Texture(Resources.MenuBG);
            background = new Sprite(backgroundTexture);
            background.Scale = new Vector2f(WindowProperties.ScaleX, WindowProperties.ScaleY);

            buttons = new List<Button>
            {
                new Button("1920x1080", new Vector2f(WindowProperties.WindowWidth * 0.3f, WindowProperties.WindowHeight * 20 / 50f)),
                new Button("1366x768",  new Vector2f(WindowProperties.WindowWidth * 0.3f, WindowProperties.WindowHeight * 26 / 50f)),
                new Button("1280x720",  new Vector2f(WindowProperties.WindowWidth * 0.3f, WindowProperties.WindowHeight * 32 / 50f)),
                new Button("1280x800",  new Vector2f(WindowProperties.WindowWidth * 0.3f, WindowProperties.WindowHeight * 38 / 50f)),
                new Button("1024x600",  new Vector2f(WindowProperties.WindowWidth * 0.3f, WindowProperties.WindowHeight * 44 / 50f)),

                new Button("Windowed 1920x1080", new Vector2f(WindowProperties.WindowWidth * 0.7f, WindowProperties.WindowHeight * 20 / 50f)),
                new Button("Windowed 1366x768",  new Vector2f(WindowProperties.WindowWidth * 0.7f, WindowProperties.WindowHeight * 26 / 50f)),
                new Button("Windowed 1280x720",  new Vector2f(WindowProperties.WindowWidth * 0.7f, WindowProperties.WindowHeight * 32 / 50f)),
                new Button("Windowed 1280x800",  new Vector2f(WindowProperties.WindowWidth * 0.7f, WindowProperties.WindowHeight * 38 / 50f)),
                new Button("Windowed 1024x600",  new Vector2f(WindowProperties.WindowWidth * 0.7f, WindowProperties.WindowHeight * 44 / 50f))
            };
        }

        public bool HasLevelEnded { get; private set; }

        public void Draw(RenderWindow window)
        {
            window.Clear(Color.Black);
            window.Draw(background);
            foreach (var button in buttons)
                button.Draw(window);
            mousePointer.Draw(window);
            window.Display();
        }

        public void LevelLogic(ref RenderWindow window)
        {
            var mousePosition = Mouse.GetPosition(window);

            mousePointer.HoversOverItemOff();
            foreach (var button in buttons)
            {
                if (button.BoundingBox.Contains(mousePosition.X, mousePosition.Y))
                {
                    mousePointer.HoversOverItemOn();
                    if (Mouse.IsButtonPressed(Mouse.Button.Left) && clock.ElapsedTime.AsMilliseconds() > 100)
                    {
                        window.Close();

                        switch (button.Label)
                        {
                            case "1920x1080":
                                window = new RenderWindow(new VideoMode(1920, 1080), "ASTROMARINES - FULL SCREEN", Styles.Fullscreen);
                                break;

                            case "1280x720":
                                window = new RenderWindow(new VideoMode(1280, 720), "ASTROMARINES - FULL SCREEN", Styles.Fullscreen);
                                break;

                            case "1366x768":
                                window = new RenderWindow(new VideoMode(1366, 768), "ASTROMARINES - FULL SCREEN", Styles.Fullscreen);
                                break;

                            case "1280x800":
                                window = new RenderWindow(new VideoMode(1280, 800), "ASTROMARINES - FULL SCREEN", Styles.Fullscreen);
                                break;

                            case "1024x600":
                                window = new RenderWindow(new VideoMode(1024, 600), "ASTROMARINES - FULL SCREEN", Styles.Fullscreen);
                                break;


                            case "Windowed 1920x1080":
                                window = new RenderWindow(new VideoMode(1920, 1080), "ASTROMARINES - FULL SCREEN", Styles.None);
                                break;

                            case "Windowed 1366x768":
                                window = new RenderWindow(new VideoMode(1280, 720), "ASTROMARINES - FULL SCREEN", Styles.Close);
                                break;

                            case "Windowed 1280x720":
                                window = new RenderWindow(new VideoMode(1366, 768), "ASTROMARINES - FULL SCREEN", Styles.Close);
                                break;

                            case "Windowed 1280x800":
                                window = new RenderWindow(new VideoMode(1280, 800), "ASTROMARINES - FULL SCREEN", Styles.Close);
                                break;

                            case "Windowed 1024x600":
                                window = new RenderWindow(new VideoMode(1024, 600), "ASTROMARINES - FULL SCREEN", Styles.Close);
                                break;
                        }

                        window.KeyPressed += Window_KeyPressed;
                        window.Closed += OnClose;
                        WindowProperties.WindowWidth = window.Size.X;
                        WindowProperties.WindowHeight = window.Size.Y;
                        window.SetFramerateLimit(60);
                        window.SetMouseCursorVisible(false);
                        window.SetVerticalSyncEnabled(true);

                        HasLevelEnded = true;
                        Mouse.SetPosition(new Vector2i((int)WindowProperties.WindowWidth / 2, (int)WindowProperties.WindowHeight / 2), window);
                    }
                }
            }

            
            
        }

        private void Window_KeyPressed(object sender, KeyEventArgs e)
        {
            if (e.Code == Keyboard.Key.Escape)
                ((RenderWindow)sender).Close();
        }

        static void OnClose(object sender, EventArgs e)
        {
            ((RenderWindow)sender).Close();
        }

        public void Dispose()
        {
            clock.Dispose();
            backgroundTexture.Dispose();
            background.Dispose();
        }
    }
}