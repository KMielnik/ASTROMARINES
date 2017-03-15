using SFML.System;

namespace ASTROMARINES
{
    public class Bullet
    {
        Vector2f Position;
        Vector2f Vector;
        private bool shouldBeDeleted = false;

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
    }
}
