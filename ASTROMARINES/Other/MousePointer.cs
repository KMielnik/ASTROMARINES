using System;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace ASTROMARINES.Other
{
    public class MousePointer : IDisposable
    {
        private readonly CircleShape _target;
        private readonly RectangleShape _targetLineX;
        private readonly RectangleShape _targetLineY;
        public MousePointer()
        {
            _target = new CircleShape(3);
            _target.Origin = new Vector2f(3, 3);
            _target.FillColor = new Color(Color.Red);
            _target.OutlineThickness = 1;
            _target.OutlineColor = new Color(Color.Black);

            _targetLineX = new RectangleShape(new Vector2f(2 * WindowProperties.WindowWidth, 1));
            _targetLineX.Origin = new Vector2f(WindowProperties.WindowWidth, 0.5f);
            _targetLineX.FillColor = new Color(Color.Red);

            _targetLineY = new RectangleShape(new Vector2f(1, 2 * WindowProperties.WindowHeight));
            _targetLineY.Origin = new Vector2f(0.5f, WindowProperties.WindowHeight);
            _targetLineY.FillColor = new Color(Color.Red);
        }

        public void Draw(RenderWindow window)
        {
            var mousePosition = (Vector2f)Mouse.GetPosition(window);
            _target.Position = mousePosition;
            _targetLineX.Position = mousePosition;
            _targetLineY.Position = mousePosition;

            window.Draw(_target);
            window.Draw(_targetLineX);
            window.Draw(_targetLineY);
        }

        public void HoversOverItemOn()
        {
            _target.FillColor = new Color(Color.Green);
            _targetLineX.FillColor = new Color(Color.Green);
            _targetLineY.FillColor = new Color(Color.Green);
        }

        public void HoversOverItemOff()
        {
            _target.FillColor = new Color(Color.Red);
            _targetLineX.FillColor = new Color(Color.Red);
            _targetLineY.FillColor = new Color(Color.Red);
        }

        public void Dispose()
        {
            _target.Dispose();
            _targetLineX.Dispose();
            _targetLineY.Dispose();
        }
    }
}
