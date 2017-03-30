using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using ASTROMARINES.Other;

namespace ASTROMARINES.Characters.Enemies
{
    class Enemy1 : ProtoEnemy, IEnemy
    {
        public enum Directions
        {
            Up, Down, Left, Right
        }

        Directions movementDirection;

        public Enemy1(List<Texture> enemyTextures) : base()
        {
            dimensions.X = 255 * 0.3f * WindowProperties.ScaleX;
            dimensions.Y = 255 * 0.3f * WindowProperties.ScaleY;

            for (int i = 0;i<6;i++)
            {
                Sprite enemyFrame = new Sprite(enemyTextures[(int)EnemyTypes.PowerUp-1]);
                enemyFrame.Origin = new Vector2f(127.5f, 127.5f);
                enemyFrame.Scale = new Vector2f(0.3f * WindowProperties.ScaleX,
                                                0.3f * WindowProperties.ScaleY);
                enemyFrame.Position = new Vector2f(WindowProperties.WindowWidth / 2,
                                                   WindowProperties.WindowHeight / 2);
                enemyFrame.TextureRect = new IntRect(i * 255, 0, 255, 255);

                enemyFrames.Add(enemyFrame);
            }

            DecideStartingPostition();

            HPMax = 1;
            HP = HPMax;
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
                    minX = (int)(dimensions.X / 2);
                    maxX = (int)(WindowProperties.WindowWidth - dimensions.X / 2);
                    randomXPosition = random.Next(minX, maxX);
                    foreach (var enemyFrame in enemyFrames)
                        enemyFrame.Position = new Vector2f(randomXPosition, 0 - dimensions.Y);
                    break;

                case Directions.Up:
                    minX = (int)(dimensions.X / 2);
                    maxX = (int)(WindowProperties.WindowWidth - dimensions.X / 2);
                    randomXPosition = random.Next(minX, maxX);
                    foreach (var enemyFrame in enemyFrames)
                        enemyFrame.Position = new Vector2f(randomXPosition, WindowProperties.WindowHeight + dimensions.Y);
                    break;

                case Directions.Right:
                    minY = (int)(dimensions.Y / 2);
                    maxY = (int)(WindowProperties.WindowHeight - dimensions.Y / 2);
                    randomYPosition = random.Next(minY, maxY);
                    foreach (var enemyFrame in enemyFrames)
                        enemyFrame.Position = new Vector2f(0 - dimensions.X, randomYPosition);
                    break;

                case Directions.Left:
                    minY = (int)(dimensions.Y / 2);
                    maxY = (int)(WindowProperties.WindowHeight - dimensions.Y / 2);
                    randomYPosition = random.Next(minY, maxY);
                    foreach (var enemyFrame in enemyFrames)
                        enemyFrame.Position = new Vector2f(WindowProperties.WindowWidth + dimensions.X,randomYPosition);
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
                    foreach (var enemyFrame in enemyFrames)
                        enemyFrame.Position += new Vector2f(0 * WindowProperties.ScaleX, 10 * WindowProperties.ScaleY);
                    break;

                case Directions.Up:
                    foreach (var enemyFrame in enemyFrames)
                        enemyFrame.Position += new Vector2f(0 * WindowProperties.ScaleX, -10 * WindowProperties.ScaleY);
                    break;

                case Directions.Right:
                    foreach (var enemyFrame in enemyFrames)
                        enemyFrame.Position += new Vector2f(10 * WindowProperties.ScaleX, 0 * WindowProperties.ScaleY);
                    break;

                case Directions.Left:
                    foreach (var enemyFrame in enemyFrames)
                        enemyFrame.Position += new Vector2f(-10 * WindowProperties.ScaleX, 0 * WindowProperties.ScaleY);
                    break;
            }

            foreach (var enemyFrame in enemyFrames)
                enemyFrame.Rotation += 8;

            CheckIfFlewOutOfMap();
        }
    }
}
