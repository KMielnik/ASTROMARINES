using System.Collections.Generic;
using ASTROMARINES.Other;
using SFML.Graphics;
using SFML.System;

namespace ASTROMARINES.Characters.Enemies
{
    internal class Enemy3 : ProtoEnemy
    {
        private readonly Cannons cannons;
        public Enemy3(Texture enemyTexture)
        {
            Dimensions.X = 255 * 0.3f * WindowProperties.ScaleX;
            Dimensions.Y = 255 * 0.3f * WindowProperties.ScaleY;

            for (var i = 0; i < 6; i++)
            {
                var enemyFrame = new Sprite(enemyTexture)
                {
                    Origin = new Vector2f(127.5f, 195.0f),
                    Scale = new Vector2f(0.3f * WindowProperties.ScaleX,
                                         0.3f * WindowProperties.ScaleY),
                    Position = RandomHorizontalPosition(),
                    TextureRect = new IntRect(i * 255, 0, 255, 390)
                };

                EnemyFrames.Add(enemyFrame);
            }

            HpBar = new HpBar(Dimensions);

            cannons = new Cannons(Position, Dimensions);

            HpMax = 12;
            Hp = HpMax;
        }

        public override void Shoot(List<Bullet> enemiesBullets)
        {
            if (ReloadingClock.ElapsedTime.AsMilliseconds() <= 30) return;

            Bullet bullet;

            cannons.CalibrateCannons(Position, Dimensions);

            switch (cannons.ActualCannon)
            {
                case 1:
                    bullet = new Bullet(cannons.Cannon1.Position, cannons.Cannon1.BulletVector);
                    enemiesBullets.Add(bullet);
                    break;

                case 2:
                    bullet = new Bullet(cannons.Cannon2.Position, cannons.Cannon2.BulletVector);
                    enemiesBullets.Add(bullet);
                    break;

                case 3:
                    bullet = new Bullet(cannons.Cannon3.Position, cannons.Cannon3.BulletVector);
                    enemiesBullets.Add(bullet);
                    break;

                case 4:
                    bullet = new Bullet(cannons.Cannon4.Position, cannons.Cannon4.BulletVector);
                    enemiesBullets.Add(bullet);
                    break;

                case 5:
                    bullet = new Bullet(cannons.Cannon5.Position, cannons.Cannon5.BulletVector);
                    enemiesBullets.Add(bullet);
                    break;

                case 6:
                    bullet = new Bullet(cannons.Cannon6.Position, cannons.Cannon6.BulletVector);
                    enemiesBullets.Add(bullet);
                    break;
            }

            ReloadingClock.Restart();
        }

        private class Cannons
        {
            public struct Cannon
            {
                public Vector2f Position;
                public readonly Vector2f BulletVector;
                public Cannon(Vector2f position, Vector2f bulletVector)
                {
                    Position = position;
                    BulletVector = bulletVector;
                }
            }

            public Cannon Cannon1;
            public Cannon Cannon2;
            public Cannon Cannon3;
            public Cannon Cannon4;
            public Cannon Cannon5;
            public Cannon Cannon6;

            private float cannonCounter;
            public int ActualCannon
            {
                get
                {
                    cannonCounter += 0.1f;
                    if (cannonCounter > 7)
                        cannonCounter = 1.0f;
                    return (int)(cannonCounter);
                }
            }

            public Cannons(Vector2f enemyPosition, Vector2f enemyDimensions)
            {
                cannonCounter = 1.0f;

                //Cannon1
                var cannonPosition = new Vector2f(enemyPosition.X + enemyDimensions.X / 3,
                    enemyPosition.Y - enemyDimensions.Y / 2);
                var bulletVector = new Vector2f(2 * WindowProperties.ScaleX,
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

            public void CalibrateCannons(Vector2f enemyPosition, Vector2f enemyDimensions)
            {
                var cannonPosition = new Vector2f(enemyPosition.X + enemyDimensions.X / 3,
                    enemyPosition.Y - enemyDimensions.Y / 2);
                Cannon1.Position = cannonPosition;

                //Cannon2
                cannonPosition = new Vector2f(enemyPosition.X,
                                              enemyPosition.Y - enemyDimensions.Y / 2);
                Cannon2.Position = cannonPosition;

                //Cannon3
                cannonPosition = new Vector2f(enemyPosition.X - enemyDimensions.X / 3,
                                              enemyPosition.Y - enemyDimensions.Y / 2);
                Cannon3.Position = cannonPosition;

                //Cannon4
                cannonPosition = new Vector2f(enemyPosition.X,
                                              enemyPosition.Y + enemyDimensions.Y / 2);
                Cannon4.Position = cannonPosition;

                //Cannon5
                cannonPosition = new Vector2f(enemyPosition.X + enemyDimensions.X / 2.4f,
                                              enemyPosition.Y + enemyDimensions.Y / 18);
                Cannon5.Position = cannonPosition;

                //Cannon6
                cannonPosition = new Vector2f(enemyPosition.X - enemyDimensions.X / 2.4f,
                                              enemyPosition.Y + enemyDimensions.Y / 18);
                Cannon6.Position = cannonPosition;
            }
        }
    }
}
