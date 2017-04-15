using System;
using System.Collections.Generic;
using ASTROMARINES.Other;
using SFML.Graphics;
using SFML.System;

namespace ASTROMARINES.Characters.Enemies
{
    internal class Enemy1 : ProtoEnemy
    {
        private Directions movementDirection;

        public Enemy1(Texture enemyTexture)
        {
            Dimensions.X = 255 * 0.3f * WindowProperties.ScaleX;
            Dimensions.Y = 255 * 0.3f * WindowProperties.ScaleY;

            for (var i = 0;i<6;i++)
            {
                var enemyFrame = new Sprite(enemyTexture);
                enemyFrame.Origin = new Vector2f(127.5f, 127.5f);
                enemyFrame.Scale = new Vector2f(0.3f * WindowProperties.ScaleX,
                                                0.3f * WindowProperties.ScaleY);
                enemyFrame.Position = new Vector2f(WindowProperties.WindowWidth / 2,
                                                   WindowProperties.WindowHeight / 2);
                enemyFrame.TextureRect = new IntRect(i * 255, 0, 255, 255);

                EnemyFrames.Add(enemyFrame);
            }

            DecideStartingPostition();

            HpMax = 1;
            Hp = HpMax;
        }

        private void DecideStartingPostition()
        {
            var random = new Random();
            movementDirection = (Directions)random.Next(0, 4);
            int minX;
            int maxX;
            int minY;
            int maxY;
            int randomXPosition;
            int randomYPosition;
            switch (movementDirection)
            {
                case Directions.Down:
                    minX = (int)(Dimensions.X / 2);
                    maxX = (int)(WindowProperties.WindowWidth - Dimensions.X / 2);
                    randomXPosition = random.Next(minX, maxX);
                    foreach (var enemyFrame in EnemyFrames)
                        enemyFrame.Position = new Vector2f(randomXPosition, 0 - Dimensions.Y);
                    break;

                case Directions.Up:
                    minX = (int)(Dimensions.X / 2);
                    maxX = (int)(WindowProperties.WindowWidth - Dimensions.X / 2);
                    randomXPosition = random.Next(minX, maxX);
                    foreach (var enemyFrame in EnemyFrames)
                        enemyFrame.Position = new Vector2f(randomXPosition, WindowProperties.WindowHeight + Dimensions.Y);
                    break;

                case Directions.Right:
                    minY = (int)(Dimensions.Y / 2);
                    maxY = (int)(WindowProperties.WindowHeight - Dimensions.Y / 2);
                    randomYPosition = random.Next(minY, maxY);
                    foreach (var enemyFrame in EnemyFrames)
                        enemyFrame.Position = new Vector2f(0 - Dimensions.X, randomYPosition);
                    break;

                case Directions.Left:
                    minY = (int)(Dimensions.Y / 2);
                    maxY = (int)(WindowProperties.WindowHeight - Dimensions.Y / 2);
                    randomYPosition = random.Next(minY, maxY);
                    foreach (var enemyFrame in EnemyFrames)
                        enemyFrame.Position = new Vector2f(WindowProperties.WindowWidth + Dimensions.X,randomYPosition);
                    break;
            }
        }

        public override void Shoot(List<Bullet> enemiesBullets)
        {
            //PowerUpIsKind = true;
        }

        public override void Move()
        {
            switch (movementDirection)
            {
                case Directions.Down:
                    foreach (var enemyFrame in EnemyFrames)
                        enemyFrame.Position += new Vector2f(0 * WindowProperties.ScaleX, 10 * WindowProperties.ScaleY);
                    break;

                case Directions.Up:
                    foreach (var enemyFrame in EnemyFrames)
                        enemyFrame.Position += new Vector2f(0 * WindowProperties.ScaleX, -10 * WindowProperties.ScaleY);
                    break;

                case Directions.Right:
                    foreach (var enemyFrame in EnemyFrames)
                        enemyFrame.Position += new Vector2f(10 * WindowProperties.ScaleX, 0 * WindowProperties.ScaleY);
                    break;

                case Directions.Left:
                    foreach (var enemyFrame in EnemyFrames)
                        enemyFrame.Position += new Vector2f(-10 * WindowProperties.ScaleX, 0 * WindowProperties.ScaleY);
                    break;
            }

            foreach (var enemyFrame in EnemyFrames)
                enemyFrame.Rotation += 8;

            CheckIfFlewOutOfMap();
        }

        public override void Draw(RenderWindow window)
        {
            var actualAnimationFrame = ActualAnimationFrame();
            window.Draw(EnemyFrames[actualAnimationFrame]);
        }
    }
}
