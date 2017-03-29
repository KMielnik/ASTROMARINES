using System;
using SFML.Graphics;
using SFML.System;
using ASTROMARINES.Properties;
using ASTROMARINES.Other;

namespace ASTROMARINES.Levels
{
    class SimpleTextScreen : ILevel
    {
        Clock clock;
        Font font;
        Text text;

        public SimpleTextScreen(string displayedText)
        {
            clock = new Clock();
            font = new Font(Resources.FontMainGameFont);
            text = new Text(displayedText, font);
            FloatRect textBoundingBox = text.GetLocalBounds();
            text.Origin = new Vector2f(textBoundingBox.Left + textBoundingBox.Width / 2,
                                       textBoundingBox.Top + textBoundingBox.Height / 2);
            text.Position = new Vector2f(WindowProperties.WindowWidth / 2, WindowProperties.WindowHeight / 2);
            text.Color = new Color(Color.White);
        }

        public bool HasLevelEnded { get => clock.ElapsedTime.AsSeconds() > 1; } //DEBUG

        public void Dispose()
        {
            font.Dispose();
            text.Dispose();
            clock.Dispose();
        }

        public void Draw(RenderWindow window)
        {
            window.Clear(Color.Black);
            window.Draw(text);
            window.Display();
        }

        public void LevelLogic(RenderWindow window)
        {

        }
    }
}