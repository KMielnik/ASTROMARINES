using SFML.Graphics;
using SFML.System;
using ASTROMARINES.Other;
using ASTROMARINES.Properties;
using System;

namespace ASTROMARINES.Levels
{
    partial class Menu
    {
        class Button : IDisposable
        {
            RectangleShape button;
            public FloatRect BoundingBox { get => button.GetGlobalBounds(); }
            Text text;
            Font font;

            public void Draw(RenderWindow window)
            {
                window.Draw(button);
                window.Draw(text);
            }

            public void Dispose()
            {
                button.Dispose();
                text.Dispose();
                font.Dispose();
            }

            public Button(string desiredText, Vector2f position)
            {
                button = new RectangleShape();
                button.Size = new Vector2f(WindowProperties.WindowWidth / 5, WindowProperties.WindowHeight / 10);
                button.FillColor = new Color(Color.White);
                button.OutlineColor = new Color(Color.Black);
                button.OutlineThickness = 5;
                button.Origin = button.Size / 2;
                button.Position = position;

                font = new Font(Resources.FontDigitalClock);
                text = new Text(desiredText, font);
                text.Color = new Color(Color.Black);
                text.CharacterSize = (uint)(button.Size.Y / 1.3f);
                FloatRect textBoundingBox = text.GetLocalBounds();
                text.Origin = new Vector2f(textBoundingBox.Left + textBoundingBox.Width / 2,
                                           textBoundingBox.Top + textBoundingBox.Height / 2);
                text.Position = button.Position;
            }
        }
    }
}