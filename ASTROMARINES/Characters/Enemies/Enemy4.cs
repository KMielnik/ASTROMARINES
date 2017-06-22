using System.Collections.Generic;
using ASTROMARINES.Other;
using SFML.Graphics;
using SFML.System;

namespace ASTROMARINES.Characters.Enemies
{
    internal class Enemy4 : ProtoEnemy
    {
        private readonly Cannons _cannons;

        public Enemy4(Texture enemyTexture)
        {
            Dimensions.X = 255 * 0.3f * WindowProperties.ScaleX;
            Dimensions.Y = 255 * 0.3f * WindowProperties.ScaleY;

            for (var i = 0; i < 6; i++)
            {
                var enemyFrame = new Sprite(enemyTexture);
                enemyFrame.Origin = new Vector2f(127.5f, 127.5f);
                enemyFrame.Scale = new Vector2f(0.3f * WindowProperties.ScaleX,
                                                0.3f * WindowProperties.ScaleY);
                enemyFrame.Position = RandomHorizontalPosition();
                enemyFrame.TextureRect = new IntRect(i * 255, 0, 255, 255);

                EnemyFrames.Add(enemyFrame);
            }

            HpBar = new HpBar(Dimensions);

            _cannons = new Cannons(Position, Dimensions);

            HpMax = 12;
            Hp = HpMax;
        }

        public override void Shoot(List<Bullet> enemiesBullets)
        {
            if (ReloadingClock.ElapsedTime.AsMilliseconds() > 5)
            {
                Bullet bullet;

                _cannons.CalibrateCannons(Position, Dimensions);

                switch (_cannons.ActualCannon)
                {
                    case 1:
                        bullet = new Bullet(_cannons.Cannon1.Position, _cannons.Cannon1.BulletVector);
                        enemiesBullets.Add(bullet);
                        break;

                    case 2:
                        bullet = new Bullet(_cannons.Cannon2.Position, _cannons.Cannon2.BulletVector);
                        enemiesBullets.Add(bullet);
                        break;

                    case 3:
                        bullet = new Bullet(_cannons.Cannon3.Position, _cannons.Cannon3.BulletVector);
                        enemiesBullets.Add(bullet);
                        break;

                    case 4:
                        bullet = new Bullet(_cannons.Cannon4.Position, _cannons.Cannon4.BulletVector);
                        enemiesBullets.Add(bullet);
                        break;

                    case 5:
                        bullet = new Bullet(_cannons.Cannon5.Position, _cannons.Cannon5.BulletVector);
                        enemiesBullets.Add(bullet);
                        break;

                    case 6:
                        bullet = new Bullet(_cannons.Cannon6.Position, _cannons.Cannon6.BulletVector);
                        enemiesBullets.Add(bullet);
                        break;
                }

                ReloadingClock.Restart();
            }
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

            private float _cannonCounter = 1.0f;
            public int ActualCannon
            {
                get
                {
                    _cannonCounter += 0.02f;
                    if (_cannonCounter > 7)
                        _cannonCounter = 1.0f;
                    return (int)(_cannonCounter);
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

            public void CalibrateCannons(Vector2f enemyPosition, Vector2f enemyDimensions)
            {
                Vector2f cannonPosition;

                cannonPosition = new Vector2f(enemyPosition.X + enemyDimensions.X / 3,
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
