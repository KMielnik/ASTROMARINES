using System;
using System.Collections.Generic;
using ASTROMARINES.Other;
using SFML.Graphics;
using SFML.System;

namespace ASTROMARINES.Characters.Enemies
{
    internal abstract class ProtoEnemy : IEnemy
    {
        protected float Hp;
        protected float HpMax;
        protected Vector2f Dimensions;
        protected List<Sprite> EnemyFrames;
        protected HpBar HpBar;
        protected Clock ReloadingClock;
        protected Clock AnimationClock;
        private bool _shouldBeDeleted;

        public ProtoEnemy()
        {
            EnemyFrames = new List<Sprite>();
            ReloadingClock = new Clock();
            AnimationClock = new Clock();
            HpBar = new HpBar(Dimensions);
        }

        public bool ShouldBeDeleted
        {
            get
            {
                if (_shouldBeDeleted)
                    return true;
                if (Hp <= 0)
                    return true;
                return false;
            }
            set
            {
                if (value)
                    _shouldBeDeleted = true;
            }
        }

        public Vector2f Position => EnemyFrames[0].Position;

        public FloatRect BoudingBox => EnemyFrames[0].GetGlobalBounds();

        protected void CheckIfFlewOutOfMap()
        {
            var flewOutOfLeftSide = Position.X < 2*(-Dimensions.X * WindowProperties.ScaleX);
            var flewOutOfRightSide = Position.X > (WindowProperties.WindowWidth + 2*(Dimensions.X * WindowProperties.ScaleX));
            var flewOutOfTheTop = Position.Y < 2*(-Dimensions.Y * WindowProperties.ScaleY);
            var flewOutOfTheBottom = Position.Y > (WindowProperties.WindowHeight + 2*(Dimensions.Y * WindowProperties.ScaleY));

            if (flewOutOfLeftSide ||
                flewOutOfRightSide ||
                flewOutOfTheBottom ||
                flewOutOfTheTop)
            {
                _shouldBeDeleted = true;
            }
        }

        protected int ActualAnimationFrame()
        {
            var timeFromLastAnimationRestart = AnimationClock.ElapsedTime.AsSeconds();
            var actualAnimationFrame = (int)(timeFromLastAnimationRestart * 5);                //new frame every 0.1 second
            if(actualAnimationFrame >= EnemyFrames.Count)
            {
                actualAnimationFrame = 0;
                AnimationClock.Restart();
            }
            return actualAnimationFrame;
        }

        protected Vector2f RandomHorizontalPosition()
        {
            var random = new Random();
            var minX = (int)(Dimensions.X / 2);
            var maxX = (int)(WindowProperties.WindowWidth - (Dimensions.X / 2));
            var newPosition = new Vector2f(random.Next(minX, maxX), 0 - Dimensions.Y);
            return newPosition;
        }

        public virtual void Shoot(List<Bullet> enemiesBullets)
        {
            throw new NotImplementedException();
        }

        public virtual void Move()
        {
            foreach(var enemyFrame in EnemyFrames)
            {
                var moveVector = new Vector2f(0, 1 * WindowProperties.ScaleY);
                enemyFrame.Position += moveVector;
            }
            CheckIfFlewOutOfMap();
        }

        public virtual void Draw(RenderWindow window)
        {
            HpBar.SetHpBarPositon(Position, Dimensions);
            var actualAnimationFrame = ActualAnimationFrame();
            window.Draw(EnemyFrames[actualAnimationFrame]);
            HpBar.Draw(window);
        }

        public void Damaged()
        {
            Hp--;
            HpBar.UpdateHpBarSize(Hp, HpMax);
        }

        public void Dispose()
        {
            foreach(var enemyFrame in EnemyFrames)
                enemyFrame.Dispose();
            HpBar.Dispose();
            ReloadingClock.Dispose();
            AnimationClock.Dispose();
        }
    }
}
