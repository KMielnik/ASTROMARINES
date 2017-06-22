using ASTROMARINES.Other;
using ASTROMARINES.Properties;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace ASTROMARINES.Levels
{
    internal class SimpleTextScreen : ILevel
    {
        private readonly Clock _clock;
        private readonly Font _font;
        private readonly Text _text;

        public SimpleTextScreen(string displayedText)
        {
            _clock = new Clock();
            _font = new Font(Resources.FontMainGameFont);
            _text = new Text(displayedText, _font);
            var textBoundingBox = _text.GetLocalBounds();
            _text.Origin = new Vector2f(textBoundingBox.Left + textBoundingBox.Width / 2,
                                       textBoundingBox.Top + textBoundingBox.Height / 2);
            _text.Position = new Vector2f(WindowProperties.WindowWidth / 2, WindowProperties.WindowHeight / 2);
            _text.Color = new Color(Color.White);
        }

        public bool HasLevelEnded { get; private set; }

        public void Dispose()
        {
            _font.Dispose();
            _text.Dispose();
            _clock.Dispose();
        }

        public void Draw(RenderWindow window)
        {
            window.Clear(Color.Black);
            window.Draw(_text);
            window.Display();
        }

        public void LevelLogic(ref RenderWindow window)
        {
            if (Mouse.IsButtonPressed(Mouse.Button.Left) && _clock.ElapsedTime.AsMilliseconds() > 200)
                HasLevelEnded = true;
            Mouse.SetPosition(new Vector2i((int)WindowProperties.WindowWidth / 2, (int)WindowProperties.WindowHeight / 2), window);
        }
    }
}