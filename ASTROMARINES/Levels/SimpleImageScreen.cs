using ASTROMARINES.Other;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace ASTROMARINES.Levels
{
    internal class SimpleImageScreen : ILevel
    {
        private readonly Clock _clock;
        private readonly Texture _texture;
        private readonly Sprite _sprite;

        public SimpleImageScreen(string imageSource)
        {
            _clock = new Clock();
            _texture = new Texture(imageSource);
            _sprite = new Sprite(_texture);
            _sprite.Scale = new Vector2f(WindowProperties.ScaleX, WindowProperties.ScaleY);
        }

        public bool HasLevelEnded { get; private set; }

        public void Draw(RenderWindow window)
        {
            window.Clear(Color.Black);
            window.Draw(_sprite);
            window.Display();
        }

        public void LevelLogic(ref RenderWindow window)
        {
            if (Mouse.IsButtonPressed(Mouse.Button.Left) && _clock.ElapsedTime.AsMilliseconds() > 200)
                HasLevelEnded = true;
            Mouse.SetPosition(new Vector2i((int)WindowProperties.WindowWidth / 2, (int)WindowProperties.WindowHeight / 2), window);
        }

        public void Dispose()
        {
            _clock.Dispose();
            _texture.Dispose();
            _sprite.Dispose();
        }
    }
}