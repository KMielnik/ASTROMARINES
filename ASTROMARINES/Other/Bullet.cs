using System;
using SFML.Graphics;
using SFML.System;

namespace ASTROMARINES.Other
{
    public class Bullet : IDisposable
    {
        public Vector2f Position { get; private set; }
        private readonly Vector2f _vector;
        private static readonly CircleShape BulletForDrawing;

        static Bullet()
        {
            BulletForDrawing = new CircleShape(3);
            BulletForDrawing.Origin = new Vector2f(3, 3);
        }

        public Bullet(Vector2f position, Vector2f vector)
        {
            Position = position;
            _vector = vector;
        }

        public bool ShouldBeDeleted { get; set; }

        public void Move()
        {
            Position += _vector;
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
            BulletForDrawing.Position = Position;
            BulletForDrawing.FillColor = Color.White;
            window.Draw(BulletForDrawing);
        }

        public void Draw(RenderWindow window, Color color)
        {
            BulletForDrawing.Position = Position;
            BulletForDrawing.FillColor = color;
            window.Draw(BulletForDrawing);
        }

        public void Dispose()
        {
        }
    }
}
