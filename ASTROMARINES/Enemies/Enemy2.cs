using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;

namespace ASTROMARINES
{
    class Enemy2 : ProtoEnemy, IEnemy
    {
        public Enemy2(List<Texture> enemyTextures)
        {
            for (int i = 0; i < 6; i++)
            {
                Sprite enemyFrame = new Sprite(enemyTextures[(int)EnemyTypes.Enemy2]);
                enemyFrame.Origin = new Vector2f(127.5f, 127.5f);
                enemyFrame.Scale = new Vector2f(0.3f * WindowProperties.WindowWidth,
                                                0.3f * WindowProperties.WindowHeight);
                enemyFrame.Position = RandomHorizontalPosition();
                enemyFrame.TextureRect = new IntRect(i * 255, 0, 255, 255);

                enemyFrames.Add(enemyFrame);
            }

            dimensions.X = 255 * 0.3f * WindowProperties.WindowWidth;
            dimensions.Y = 255 * 0.3f * WindowProperties.WindowHeight;

            SetUpHPBar();

            HPMax = 6;
            HP = HPMax;
        }

        public override void Shoot(List<Bullet> EnemiesBullets)
        {
            if (reloadingClock.ElapsedTime.AsMilliseconds() > 200)
            {
                Cannons cannons = new Cannons(Position, dimensions);
                Bullet bullet;

                bullet = new Bullet(cannons.Cannon1.Position, cannons.Cannon1.BulletVector);
                EnemiesBullets.Add(bullet);

                bullet = new Bullet(cannons.Cannon2.Position, cannons.Cannon2.BulletVector);
                EnemiesBullets.Add(bullet);

                bullet = new Bullet(cannons.Cannon3.Position, cannons.Cannon3.BulletVector);
                EnemiesBullets.Add(bullet);

                bullet = new Bullet(cannons.Cannon4.Position, cannons.Cannon4.BulletVector);
                EnemiesBullets.Add(bullet);

                reloadingClock.Restart();
            }
        }

        class Cannons
        {
            public struct Cannon
            {
                public Vector2f Position;
                public Vector2f BulletVector;
                public Cannon(Vector2f position,Vector2f bulletVector)
                {
                    Position = position;
                    BulletVector = bulletVector;
                }
            }

            public Cannon Cannon1 { get; private set; }
            public Cannon Cannon2 { get; private set; }
            public Cannon Cannon3 { get; private set; }
            public Cannon Cannon4 { get; private set; }

            public Cannons(Vector2f enemyPosition,Vector2f enemyDimensions)
            {
                Vector2f cannonPosition;
                Vector2f bulletVector;
                
                //Cannon1
                cannonPosition = new Vector2f(enemyPosition.X + enemyDimensions.X / 3,
                                              enemyPosition.Y - enemyDimensions.Y / 10);
                bulletVector = new Vector2f(3 * WindowProperties.ScaleX,
                                            0 * WindowProperties.ScaleY);
                Cannon1 = new Cannon(cannonPosition, bulletVector);

                //Cannon2
                cannonPosition = new Vector2f(enemyPosition.X + enemyDimensions.X / 3,
                                              enemyPosition.Y + enemyDimensions.Y / 3);
                bulletVector = new Vector2f(3 * WindowProperties.ScaleX,
                                            3 * WindowProperties.ScaleY);
                Cannon2 = new Cannon(cannonPosition, bulletVector);

                //Cannon3
                cannonPosition = new Vector2f(enemyPosition.X - enemyDimensions.X / 3,
                                              enemyPosition.Y - enemyDimensions.Y / 10);
                bulletVector = new Vector2f(-3 * WindowProperties.ScaleX,
                                             0 * WindowProperties.ScaleY);
                Cannon3 = new Cannon(cannonPosition, bulletVector);

                //Cannon4
                cannonPosition = new Vector2f(enemyPosition.X - enemyDimensions.X / 3,
                                              enemyPosition.Y + enemyDimensions.Y / 3);
                bulletVector = new Vector2f(-3 * WindowProperties.ScaleX,
                                             3 * WindowProperties.ScaleY);
                Cannon4 = new Cannon(cannonPosition, bulletVector);
            }
        }
    }
}
