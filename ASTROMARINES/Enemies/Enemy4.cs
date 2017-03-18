using SFML.Graphics;
using SFML.System;
using System.Collections.Generic;

namespace ASTROMARINES
{
    class Enemy4 : ProtoEnemy, IEnemy
    {
        public Enemy4(List<Texture> enemyTextures) : base()
        {
            for (int i = 0; i < 6; i++)
            {
                Sprite enemyFrame = new Sprite(enemyTextures[(int)EnemyTypes.Enemy4]);
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

            HPMax = 12;
            HP = HPMax;
        }

        public override void Shoot(List<Bullet> EnemiesBullets)
        {
            if (reloadingClock.ElapsedTime.AsMilliseconds() > 35)
            {
                Cannons cannons = new Cannons(Position, dimensions);
                Bullet bullet;

                switch (cannons.ActualCannon)
                {
                    case 1:
                        bullet = new Bullet(cannons.Cannon1.Position, cannons.Cannon1.BulletVector);
                        EnemiesBullets.Add(bullet);
                        break;

                    case 2:
                        bullet = new Bullet(cannons.Cannon2.Position, cannons.Cannon2.BulletVector);
                        EnemiesBullets.Add(bullet);
                        break;

                    case 3:
                        bullet = new Bullet(cannons.Cannon3.Position, cannons.Cannon3.BulletVector);
                        EnemiesBullets.Add(bullet);
                        break;

                    case 4:
                        bullet = new Bullet(cannons.Cannon4.Position, cannons.Cannon4.BulletVector);
                        EnemiesBullets.Add(bullet);
                        break;

                    case 5:
                        bullet = new Bullet(cannons.Cannon5.Position, cannons.Cannon5.BulletVector);
                        EnemiesBullets.Add(bullet);
                        break;

                    case 6:
                        bullet = new Bullet(cannons.Cannon6.Position, cannons.Cannon6.BulletVector);
                        EnemiesBullets.Add(bullet);
                        break;
                }

                reloadingClock.Restart();
            }
        }

        class Cannons
        {
            public struct Cannon
            {
                public Vector2f Position;
                public Vector2f BulletVector;
                public Cannon(Vector2f position, Vector2f bulletVector)
                {
                    Position = position;
                    BulletVector = bulletVector;
                }
            }

            public Cannon Cannon1 { get; private set; }
            public Cannon Cannon2 { get; private set; }
            public Cannon Cannon3 { get; private set; }
            public Cannon Cannon4 { get; private set; }
            public Cannon Cannon5 { get; private set; }
            public Cannon Cannon6 { get; private set; }

            private float CannonCounter = 1.0f;
            public int ActualCannon
            {
                get
                {
                    CannonCounter += 0.4f;
                    if (CannonCounter > 7)
                        CannonCounter = 1.0f;
                    return (int)(CannonCounter);
                }
            }

            public Cannons(Vector2f enemyPosition, Vector2f enemyDimensions)
            {
                Vector2f cannonPosition;
                Vector2f bulletVector;

                //Cannon1
                cannonPosition = new Vector2f(enemyPosition.X + enemyDimensions.X / 3,
                                              enemyPosition.Y - enemyDimensions.Y / 2);
                bulletVector = new Vector2f(2 * WindowProperties.ScaleX,
                                            -4.7f * WindowProperties.ScaleY);
                Cannon1 = new Cannon(cannonPosition, bulletVector);

                //Cannon2
                cannonPosition = new Vector2f(enemyPosition.X,
                                              enemyPosition.Y - enemyDimensions.Y / 2);
                bulletVector = new Vector2f(0 * WindowProperties.ScaleX,
                                            -4.7f * WindowProperties.ScaleY);
                Cannon2 = new Cannon(cannonPosition, bulletVector);

                //Cannon3
                cannonPosition = new Vector2f(enemyPosition.X - enemyDimensions.X / 3,
                                              enemyPosition.Y - enemyDimensions.Y / 2);
                bulletVector = new Vector2f(-2 * WindowProperties.ScaleX,
                                             -4.7f * WindowProperties.ScaleY);
                Cannon3 = new Cannon(cannonPosition, bulletVector);

                //Cannon4
                cannonPosition = new Vector2f(enemyPosition.X,
                                              enemyPosition.Y + enemyDimensions.Y / 2);
                bulletVector = new Vector2f(0 * WindowProperties.ScaleX,
                                            4.7f * WindowProperties.ScaleY);
                Cannon4 = new Cannon(cannonPosition, bulletVector);

                //Cannon5
                cannonPosition = new Vector2f(enemyPosition.X + enemyDimensions.X / 2.4f,
                                              enemyPosition.Y + enemyDimensions.Y / 18);
                bulletVector = new Vector2f(5.13f * WindowProperties.ScaleX,
                                            1 * WindowProperties.ScaleY);
                Cannon5 = new Cannon(cannonPosition, bulletVector);

                //Cannon6
                cannonPosition = new Vector2f(enemyPosition.X - enemyDimensions.X / 2.4f,
                                              enemyPosition.Y + enemyDimensions.Y / 18);
                bulletVector = new Vector2f(-5.13f * WindowProperties.ScaleX,
                                            1 * WindowProperties.ScaleY);
                Cannon6 = new Cannon(cannonPosition, bulletVector);
            }
        }
    }
}
