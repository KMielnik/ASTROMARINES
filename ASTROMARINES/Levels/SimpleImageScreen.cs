using ASTROMARINES.Other;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace ASTROMARINES.Levels
{
    internal class SimpleImageScreen : ILevel
    {
        private readonly Clock clock;
        private readonly Texture texture;
        private readonly Sprite sprite;

        public SimpleImageScreen(string imageSource)
        {
            clock = new Clock();
            texture = new Texture(imageSource);
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
            if (Mouse.IsButtonPressed(Mouse.Button.Left) && clock.ElapsedTime.AsMilliseconds() > 200)
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