using System;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace ASTROMARINES.Other
{
    public class MousePointer : IDisposable
    {
        private readonly CircleShape target;
        private readonly RectangleShape targetLineX;
        private readonly RectangleShape targetLineY;
        public MousePointer()
        {
            target = new CircleShape(3)
            {
                Origin = new Vector2f(3, 3),
                FillColor = new Color(Color.Red),
                OutlineThickness = 1,
                OutlineColor = new Color(Color.Black)
            };

            targetLineX = new RectangleShape(new Vector2f(2 * WindowProperties.WindowWidth, 1))
            {
                Origin = new Vector2f(WindowProperties.WindowWidth, 0.5f),
                FillColor = new Color(Color.Red)
            };

            targetLineY = new RectangleShape(new Vector2f(1, 2 * WindowProperties.WindowHeight))
            {
                Origin = new Vector2f(0.5f, WindowProperties.WindowHeight),
                FillColor = new Color(Color.Red)
            };
        }

        public void Draw(RenderWindow window)
        {
            var mousePosition = (Vector2f)Mouse.GetPosition(window);
            target.Position = mousePosition;
            targetLineX.Position = mousePosition;
            targetLineY.Position = mousePosition;

            window.Draw(target);
            window.Draw(targetLineX);
            window.Draw(targetLineY);
        }

        public void HoversOverItemOn()
        {
            target.FillColor = new Color(Color.Green);
            targetLineX.FillColor = new Color(Color.Green);
            targetLineY.FillColor = new Color(Color.Green);
        }

        public void HoversOverItemOff()
        {
            target.FillColor = new Color(Color.Red);
            targetLineX.FillColor = new Color(Color.Red);
            targetLineY.FillColor = new Color(Color.Red);
        }

        public void Dispose()
        {
            target.Dispose();
            targetLineX.Dispose();
            targetLineY.Dispose();
        }
    }
}
