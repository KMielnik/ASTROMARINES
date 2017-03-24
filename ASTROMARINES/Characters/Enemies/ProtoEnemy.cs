using SFML.Graphics;
using SFML.System;
using System.Collections.Generic;
using System;
using ASTROMARINES.Other;

namespace ASTROMARINES.Characters.Enemies
{
    abstract class ProtoEnemy : IEnemy
    {
        protected float HP;
        protected float HPMax;
        protected Vector2f dimensions;
        protected List<Sprite> enemyFrames;
        protected HPBar hpBar;
        protected Clock reloadingClock;
        protected Clock animationClock;
        protected bool shouldBeDeleted;

        public ProtoEnemy()
        {
            enemyFrames = new List<Sprite>();
            reloadingClock = new Clock();
            animationClock = new Clock();
            hpBar = new HPBar(dimensions);
        }

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

        public Vector2f Position { get => enemyFrames[0].Position; private set { Position = value; } }

        public FloatRect BoudingBox { get => enemyFrames[0].GetGlobalBounds(); }

        protected void CheckIfFlewOutOfMap()
        {
            bool FlewOutOfLeftSide = Position.X < (-dimensions.X * WindowProperties.ScaleX);
            bool FlewOutOfRightSide = Position.X > (WindowProperties.WindowWidth + (dimensions.X * WindowProperties.ScaleX));
            bool FlewOutOfTheTop = Position.Y < (-dimensions.Y * WindowProperties.ScaleY);
            bool FlewOutOfTheBottom = Position.Y > (WindowProperties.WindowHeight + (dimensions.Y * WindowProperties.ScaleY));

            if (FlewOutOfLeftSide ||
                FlewOutOfRightSide ||
                FlewOutOfTheBottom ||
                FlewOutOfTheTop)
            {
                shouldBeDeleted = true;
            }
        }

        protected int ActualAnimationFrame()
        {
            var timeFromLastAnimationRestart = animationClock.ElapsedTime.AsSeconds();
            var actualAnimationFrame = (int)(timeFromLastAnimationRestart * 10);                //new frame every 0.1 second
            if(actualAnimationFrame > enemyFrames.Count)
            {
                actualAnimationFrame = 0;
                animationClock.Restart();
            }
            return actualAnimationFrame;
        }

        protected Vector2f RandomHorizontalPosition()
        {
            Random random = new Random();
            var minX = (int)(dimensions.X / 2);
            var maxX = (int)(WindowProperties.WindowWidth - (dimensions.X / 2));
            var newPosition = new Vector2f((float)random.Next(minX, maxX), 0 - dimensions.Y);
            return newPosition;
        }

        public virtual void Shoot(List<Bullet> EnemiesBullets)
        {
            throw new NotImplementedException();
        }

        public virtual void Move()
        {
            foreach(var enemyFrame in enemyFrames)
            {
                var moveVector = new Vector2f(0, 1 * WindowProperties.ScaleY);
                Position += moveVector;
            }
            CheckIfFlewOutOfMap();
        }

        public void DrawEnemy(RenderWindow window)
        {
            hpBar.SetHPBarPositon(Position, dimensions);
            window.Draw(enemyFrames[ActualAnimationFrame()]);
            hpBar.Draw(window);
        }

        public void Damaged()
        {
            HP--;
            hpBar.UpdateHPBarSize(HP, HPMax);
        }
    }
}
