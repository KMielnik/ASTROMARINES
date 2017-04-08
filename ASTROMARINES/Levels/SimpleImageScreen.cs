using SFML.Graphics;
using SFML.System;
using ASTROMARINES.Properties;
using ASTROMARINES.Other;
using System;
using SFML.Window;

namespace ASTROMARINES.Levels
{
    class SimpleImageScreen : ILevel, IDisposable
    {
        Clock clock;
        Texture texture;
        Sprite sprite;

        public SimpleImageScreen(string imageSource)
        {
            clock = new Clock();
            texture = new Texture(imageSource);
            sprite = new Sprite(texture);
            FloatRect spriteBoundingBox = sprite.GetLocalBounds();
            sprite.Scale = new Vector2f(WindowProperties.ScaleX, WindowProperties.ScaleY);
        }

        public bool HasLevelEnded { get; private set; }

        public void Draw(RenderWindow window)
        {
            window.Clear(Color.Black);
            window.Draw(sprite);
            window.Display();
        }

        public void LevelLogic(RenderWindow window)
        {
            if (Mouse.IsButtonPressed(Mouse.Button.Left) && clock.ElapsedTime.AsMilliseconds() > 100)
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