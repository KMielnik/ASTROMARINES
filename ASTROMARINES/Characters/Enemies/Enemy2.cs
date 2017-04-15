using System.Collections.Generic;
using ASTROMARINES.Other;
using SFML.Graphics;
using SFML.System;

namespace ASTROMARINES.Characters.Enemies
{
    internal class Enemy2 : ProtoEnemy
    {
        public Enemy2(Texture enemyTexture)
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

            HpMax = 6;
            Hp = HpMax;
        }

        public override void Shoot(List<Bullet> enemiesBullets)
        {
            if (ReloadingClock.ElapsedTime.AsMilliseconds() > 200)
            {
                var cannons = new Cannons(Position, Dimensions);
                Bullet bullet;

                bullet = new Bullet(cannons.Cannon1.Position, cannons.Cannon1.BulletVector);
                enemiesBullets.Add(bullet);

                bullet = new Bullet(cannons.Cannon2.Position, cannons.Cannon2.BulletVector);
                enemiesBullets.Add(bullet);

                bullet = new Bullet(cannons.Cannon3.Position, cannons.Cannon3.BulletVector);
                enemiesBullets.Add(bullet);

                bullet = new Bullet(cannons.Cannon4.Position, cannons.Cannon4.BulletVector);
                enemiesBullets.Add(bullet);

                ReloadingClock.Restart();
            }
        }

        private class Cannons
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
