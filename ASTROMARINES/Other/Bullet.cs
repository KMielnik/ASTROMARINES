using SFML.Graphics;
using SFML.System;

namespace ASTROMARINES.Other
{
    public class Bullet
    {
        public Vector2f Position { get; private set; }
        Vector2f Vector;
        static CircleShape bulletForDrawing;
        private bool shouldBeDeleted = false;

        static Bullet()
        {
            bulletForDrawing = new CircleShape(3);
            bulletForDrawing.Origin = new Vector2f(3, 3);
        }

        public Bullet(Vector2f position, Vector2f vector)
        {
            Position = position;
            Vector = vector;
        }

        public bool ShouldBeDeleted { get => shouldBeDeleted; set => shouldBeDeleted = value; }

        public void Move()
        {
            Position += Vector;
            CheckIfFlewOutOfMap();
        }

        private void CheckIfFlewOutOfMap()
        {
            bool FlewOutOfLeftSide = Position.X < (-30 * WindowProperties.ScaleX);
            bool FlewOutOfRightSide = Position.X > (WindowProperties.WindowWidth + (30 * WindowProperties.ScaleX));
            bool FlewOutOfTheTop = Position.Y < (-30 * WindowProperties.ScaleY);
            bool FlewOutOfTheBottom = Position.Y > (WindowProperties.WindowHeight + (30 * WindowProperties.ScaleY));

            if (FlewOutOfLeftSide  ||
                FlewOutOfRightSide ||
                FlewOutOfTheBottom ||
                FlewOutOfTheTop)
            {
                ShouldBeDeleted = true;
            }
        }

        public void Draw(RenderWindow window)
        {
            bulletForDrawing.Position = Position;
            window.Draw(bulletForDrawing);
        }

        public void Draw(RenderWindow window, Color color)
        {
            bulletForDrawing.Position = Position;
            bulletForDrawing.FillColor = color;
            window.Draw(bulletForDrawing);
        }
    }
}
