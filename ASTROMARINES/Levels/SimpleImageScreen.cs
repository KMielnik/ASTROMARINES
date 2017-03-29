using SFML.Graphics;
using SFML.System;
using ASTROMARINES.Properties;
using ASTROMARINES.Other;
using System;

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
            sprite.Origin = new Vector2f(spriteBoundingBox.Left + spriteBoundingBox.Width / 2,
                                       spriteBoundingBox.Top + spriteBoundingBox.Height / 2);
            sprite.Position = new Vector2f(WindowProperties.WindowWidth / 2, WindowProperties.WindowHeight / 2);
        }

        public bool HasLevelEnded { get => clock.ElapsedTime.AsSeconds() > 2; } //DEBUG

        public void Draw(RenderWindow window)
        {
            window.Clear(Color.Black);
            window.Draw(sprite);
            window.Display();
        }

        public void LevelLogic(RenderWindow window)
        {

        }

        public void Dispose()
        {
            clock.Dispose();
            texture.Dispose();
            sprite.Dispose();
        }
    }
}