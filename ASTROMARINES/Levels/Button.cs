using System;
using ASTROMARINES.Other;
using ASTROMARINES.Properties;
using SFML.Graphics;
using SFML.System;

namespace ASTROMARINES.Levels
{
    class Button : IDisposable
    {
        private readonly RectangleShape _button;
        public FloatRect BoundingBox => _button.GetGlobalBounds();
        private readonly Text _text;
        private readonly Font _font;
        public string Label => _text.DisplayedString;

        public void Draw(RenderWindow window)
        {
            window.Draw(_button);
            window.Draw(_text);
        }

        public void Dispose()
        {
            _button.Dispose();
            _text.Dispose();
            _font.Dispose();
        }

        public Button(string desiredText, Vector2f position)
        {
            _button = new RectangleShape();
            _button.Size = new Vector2f(WindowProperties.WindowWidth / 3, WindowProperties.WindowHeight / 12);
            _button.FillColor = new Color(Color.White);
            _button.OutlineColor = new Color(Color.Black);
            _button.OutlineThickness = 5;
            _button.Origin = _button.Size / 2;
            _button.Position = position;

            _font = new Font(Resources.FontDigitalClock);
            _text = new Text(desiredText, _font)
            {
                Color = new Color(Color.Black),
                CharacterSize = (uint)(_button.Size.Y / 1.3f)
            };
            var textBoundingBox = _text.GetLocalBounds();
            _text.Origin = new Vector2f(textBoundingBox.Left + textBoundingBox.Width / 2,
                                       textBoundingBox.Top + textBoundingBox.Height / 2);
            _text.Position = _button.Position;
        }

        public void SetPosition(Vector2f position)
        {
            _button.Position = position;
            _button.Origin = _button.Size / 2;
            _button.Size = new Vector2f(WindowProperties.WindowWidth / 3, WindowProperties.WindowHeight / 12);
            _text.CharacterSize = (uint)(_button.Size.Y / 1.3f);
            var textBoundingBox = _text.GetLocalBounds();
            _text.Origin = new Vector2f(textBoundingBox.Left + textBoundingBox.Width / 2,
                                       textBoundingBox.Top + textBoundingBox.Height / 2);
            _text.Position = _button.Position;
        }
    }
}