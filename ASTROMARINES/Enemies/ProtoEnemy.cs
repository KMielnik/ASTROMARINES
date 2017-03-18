﻿using SFML.Graphics;
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

        public ProtoEnemy()
        {
            enemyFrames = new List<Sprite>();
            HPBar = new RectangleShape();
            HPBarBackground = new RectangleShape();
            reloadingClock = new Clock();
            animationClock = new Clock();
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
        protected void SetHPBarPosition()
        {
            var newHPBarPosition = new Vector2f(Position.X, Position.Y + (dimensions.Y * 7 / 10));
            HPBar.Position = newHPBarPosition;
            HPBarBackground.Position = newHPBarPosition;
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

        protected void SetUpHPBar()
        {
            HPBar.Size = new Vector2f(dimensions.X, 3);
            HPBar.Origin = new Vector2f(dimensions.X / 2, 1.5f);
            HPBar.FillColor = new Color(Color.Red);
            HPBarBackground.Size = new Vector2f(dimensions.X + 2, 5);
            HPBarBackground.Origin = new Vector2f(dimensions.X / 2 + 1, 2.5f);
            HPBarBackground.FillColor = new Color(Color.White);
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
            SetHPBarPosition();
            window.Draw(enemyFrames[ActualAnimationFrame()]);
            window.Draw(HPBarBackground);
            window.Draw(HPBar);
        }

        public void Damaged()
        {
            HP--;
            UpdateHPBarSize();
        }

        private void UpdateHPBarSize()
        {
            var newHPBarSize = new Vector2f((HPBarBackground.Size.X - 2) * HP / HPMax, 3);
            HPBar.Size = newHPBarSize;
        }
    }
}