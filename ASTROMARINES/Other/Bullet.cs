using System;
using SFML.Graphics;
using SFML.System;

namespace ASTROMARINES.Other
{
    public class Bullet : IDisposable
    {
        public Vector2f Position { get; private set; }
        private Vector2f vector;
        private static CircleShape bulletForDrawing;
        private bool shouldBeDeleted;

        static Bullet()
        {
            bulletForDrawing = new CircleShape(3);
            bulletForDrawing.Origin = new Vector2f(3, 3);
        }

        public Bullet(Vector2f position, Vector2f vector)
        {
            Position = position;
            this.vector = vector;
        }

        public bool ShouldBeDeleted { get => shouldBeDeleted; set => shouldBeDeleted = value; }

        public void Move()
        {
            Position += vector;
            CheckIfFlewOutOfMap();
        }

        private void CheckIfFlewOutOfMap()
        {
            var flewOutOfLeftSide = Position.X < (-30 * WindowProperties.ScaleX);
            var flewOutOfRightSide = Position.X > (WindowProperties.WindowWidth + (30 * WindowProperties.ScaleX));
            var flewOutOfTheTop = Position.Y < (-30 * WindowProperties.ScaleY);
            var flewOutOfTheBottom = Position.Y > (WindowProperties.WindowHeight + (30 * WindowProperties.ScaleY));

            if (flewOutOfLeftSide  ||
                flewOutOfRightSide ||
                flewOutOfTheBottom ||
                flewOutOfTheTop)
            {
                ShouldBeDeleted = true;
            }
        }

        public void Draw(RenderWindow window)
        {
            bulletForDrawing.Position = Position;
            bulletForDrawing.FillColor = Color.White;
            window.Draw(bulletForDrawing);
        }

        public void Draw(RenderWindow window, Color color)
        {
            bulletForDrawing.Position = Position;
            bulletForDrawing.FillColor = color;
            window.Draw(bulletForDrawing);
        }

        public void Dispose()
        {
        }
    }
}
