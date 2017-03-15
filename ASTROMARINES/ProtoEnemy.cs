using SFML.Graphics;
using SFML.System;
using System.Collections.Generic;
using System;

namespace ASTROMARINES
{
    abstract class ProtoEnemy : IEnemy
    {
        protected float HP;
        protected float HPMax;
        protected Vector2f dimensions;
        protected List<Sprite> enemyFrames;
        protected RectangleShape HPBar;
        protected RectangleShape HPBarBackground;
        protected Clock reloadingClock;
        protected Clock animationClock;
        protected bool shouldBeDeleted;

        public bool ShouldBeDeleted
        {
            get
            {
                if (shouldBeDeleted == true)
                    return true;
                if (HP <= 0)
                    return true;
                return false;
            }
            set
            {
                if (value == true)
                    shouldBeDeleted = true;
            }
        }

        public Vector2f Position { get => enemyFrames[0].Position; }

        public FloatRect BoudingBox { get => enemyFrames[0].GetGlobalBounds(); }

        protected void CheckIfFlewOutOfMap()
        {
            bool FlewOutOfLeftSide = Position.X < (-50 * WindowProperties.ScaleX);
            bool FlewOutOfRightSide = Position.X > (WindowProperties.WindowWidth + (50 * WindowProperties.ScaleX));
            bool FlewOutOfTheTop = Position.Y < (-50 * WindowProperties.ScaleY);
            bool FlewOutOfTheBottom = Position.Y > (WindowProperties.WindowHeight + (50 * WindowProperties.ScaleY));

            if (FlewOutOfLeftSide ||
                FlewOutOfRightSide ||
                FlewOutOfTheBottom ||
                FlewOutOfTheTop)
            {
                shouldBeDeleted = true;
            }
        }
        protected void SetHPBarPosition()
        {
            var newHPBarPosition = new Vector2f(Position.X, Position.Y + (dimensions.Y * 7 / 10));
            HPBar.Position = newHPBarPosition;
            HPBarBackground.Position = newHPBarPosition;
        }
        protected void ActualAnimationFrame()
        {
            var timeFromLastAnimationRestart = animationClock.ElapsedTime.AsSeconds();
        }

        public void Shoot(List<Bullet> EnemiesBullets)
        {
            throw new NotImplementedException();
        }

        public void Move()
        {
            throw new NotImplementedException();
        }

        public void DrawEnemy(RenderWindow window)
        {
            throw new NotImplementedException();
        }

        public void Damaged()
        {
            throw new NotImplementedException();
        }
    }
}
