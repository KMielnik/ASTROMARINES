using ASTROMARINES.Other;
using ASTROMARINES.Properties;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace ASTROMARINES.Levels
{
    internal class Settings : ILevel
    {
        private Clock clock;
        private Texture texture;
        private Sprite sprite;

        public Settings(string nonUsed)
        {
            clock = new Clock();
            texture = new Texture(Resources.MenuBG);
            sprite = new Sprite(texture);
            sprite.Scale = new Vector2f(WindowProperties.ScaleX, WindowProperties.ScaleY);
        }

        public bool HasLevelEnded { get; private set; }

        public void Draw(RenderWindow window)
        {
            window.Clear(Color.Black);
            window.Draw(sprite);
            window.Display();
        }

        public void LevelLogic(ref RenderWindow window)
        {
            window.Close();
            
            window = new RenderWindow(new VideoMode(1920, 1080), "ASTROMARINES - FULL SCREEN", Styles.Fullscreen);
            window.SetActive();
            WindowProperties.WindowWidth = 1920;
            WindowProperties.WindowHeight = 1080;
            window.SetFramerateLimit(60);
            window.SetMouseCursorVisible(false);
            window.SetVerticalSyncEnabled(true);
            HasLevelEnded = true;
            Mouse.SetPosition(new Vector2i((int)WindowProperties.WindowWidth / 2, (int)WindowProperties.WindowHeight / 2), window);
            
        }

        public void Dispose()
        {
            clock.Dispose();
            texture.Dispose();
            sprite.Dispose();
        }
    }
}