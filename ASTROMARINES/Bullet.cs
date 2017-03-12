namespace ASTROMARINES
{
    public class Bullet
    {
        SFML.System.Vector2f Position;
        SFML.System.Vector2f Vector;
        private bool outOfMap = false;

        Bullet(SFML.System.Vector2f position, SFML.System.Vector2f vector)
        {
            Position = position;
            Vector = vector;
        }

        public bool OutOfMap { get => outOfMap; }

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
                outOfMap = true;
            }
        }
    }
}
