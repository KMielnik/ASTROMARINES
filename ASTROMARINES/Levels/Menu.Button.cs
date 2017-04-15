using System;
using ASTROMARINES.Other;
using ASTROMARINES.Properties;
using SFML.Graphics;
using SFML.System;

namespace ASTROMARINES.Levels
{
    internal partial class Menu
    {
        private class Button : IDisposable
        {
            private RectangleShape button;
            public FloatRect BoundingBox => button.GetGlobalBounds();
            private Text text;
            private Font font;
            public string Label => text.DisplayedString;

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
                button.Size = new Vector2f(WindowProperties.WindowWidth / 4, WindowProperties.WindowHeight / 12);
                button.FillColor = new Color(Color.White);
                button.OutlineColor = new Color(Color.Black);
                button.OutlineThickness = 5;
                button.Origin = button.Size / 2;
                button.Position = position;

                font = new Font(Resources.FontDigitalClock);
                text = new Text(desiredText, font);
                text.Color = new Color(Color.Black);
                text.CharacterSize = (uint)(button.Size.Y / 1.3f);
                var textBoundingBox = text.GetLocalBounds();
                text.Origin = new Vector2f(textBoundingBox.Left + textBoundingBox.Width / 2,
                                           textBoundingBox.Top + textBoundingBox.Height / 2);
                text.Position = button.Position;
            }
        }
    }
}