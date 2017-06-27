using System;
using System.Collections.Generic;
using ASTROMARINES.Other;
using ASTROMARINES.Properties;
using SFML.Audio;
using SFML.Graphics;
using SFML.System;

namespace ASTROMARINES.Characters.Enemies
{
    internal sealed class Boss : IEnemy
    {
        private struct DirectionsVector
        {
            public Directions X;
            public Directions Y;
            public DirectionsVector(Directions x,Directions y)
            {
                X = x;
                Y = y;
            }
        }

        private DirectionsVector movementDirection;
        private Vector2f howMuchBossFliedInOneDirection;
        private readonly float hpMax;
        // ReSharper disable once NotAccessedField.Local
        private Vector2f dimensions;
        private readonly List<Sprite> bossFrames;
        private readonly List<Sprite> backgroundBossFrames;
        private readonly HpBar hpBar;
        private readonly Clock reloadingClock;
        private readonly Clock animationClock;
        private readonly Clock bossClock;
        private readonly Random random;
        private readonly Music changingAttackSound;

        private int shootingMethod;

        private bool shouldBeDeleted;

        public Boss(Texture bossTexture, Texture backgroundBossTexture)
        {
            dimensions = new Vector2f
            {
                X = 512 * 0.7f * WindowProperties.ScaleX,
                Y = 204 * 0.7f * WindowProperties.ScaleY
            };

            bossFrames = new List<Sprite>();
            backgroundBossFrames = new List<Sprite>();

            for (var i = 0; i < 6; i++)
            {
                {
                    var enemyFrame = new Sprite(bossTexture)
                    {
                        Origin = new Vector2f(256f, 192f),
                        Scale = new Vector2f(0.7f * WindowProperties.ScaleX,
                                             0.7f * WindowProperties.ScaleY),
                        Position = new Vector2f(WindowProperties.WindowWidth / 2, WindowProperties.WindowHeight / 7),
                        TextureRect = new IntRect(i * 512, 0, 512, 204)
                    };

                    bossFrames.Add(enemyFrame);
                }

                {
                    var backgroundEnemyFrame = new Sprite(backgroundBossTexture)
                    {
                        Origin = new Vector2f(500f, 104f),
                        Scale = new Vector2f(0.7f * WindowProperties.ScaleX,
                                             0.7f * WindowProperties.ScaleY),
                        Position = new Vector2f(WindowProperties.WindowWidth / 2, WindowProperties.WindowHeight / 7),
                        TextureRect = new IntRect(i * 1000, 0, 1000, 208)
                    };

                    backgroundBossFrames.Add(backgroundEnemyFrame);
                }
            }
            movementDirection = new DirectionsVector(Directions.Left,
                                                     Directions.Down);

            hpBar = new HpBar(new Vector2f(WindowProperties.WindowWidth / 1.3f,
                                           WindowProperties.WindowHeight / 15));

            changingAttackSound = new Music(Resources.BossWarpSound);
            changingAttackSound.Stop();

            reloadingClock = new Clock();
            animationClock = new Clock();
            bossClock = new Clock();

            random = new Random();

            howMuchBossFliedInOneDirection = new Vector2f(WindowProperties.WindowWidth / 4,
                                                          WindowProperties.WindowHeight / 8);

            hpMax = 3000;
            Hp = hpMax;
        }

        public bool ShouldBeDeleted
        {
            get
            {
                if (shouldBeDeleted)
                    return true;
                if (Hp <= 0)
                    return true;
                return false;
            }
            set
            {
                if (value)
                    shouldBeDeleted = true;
            }
        }

        private int ActualAnimationFrame()
        {
            var timeFromLastAnimationRestart = animationClock.ElapsedTime.AsSeconds();
            var actualAnimationFrame = (int)(timeFromLastAnimationRestart * 5);                //new frame every 0.1 second
            if (actualAnimationFrame >= bossFrames.Count)
            {
                actualAnimationFrame = 0;
                animationClock.Restart();
            }
            return actualAnimationFrame;
        }

        public Vector2f Position => bossFrames[0].Position;

        public FloatRect BoudingBox => bossFrames[0].GetGlobalBounds();

        private float Hp { get; set; }

        public void Damaged()
        {
            Hp--;
            hpBar.UpdateHpBarSize(Hp, hpMax);
        }

        public void Dispose()
        {
            foreach (var bossFrame in bossFrames)
                bossFrame.Dispose();
            foreach (var backgroundBossFrame in backgroundBossFrames)
                backgroundBossFrame.Dispose();

            changingAttackSound.Dispose();

            hpBar.Dispose();
            reloadingClock.Dispose();
            animationClock.Dispose();
        }

        public void Draw(RenderWindow window)
        {
            var healthBarPosition = new Vector2f(WindowProperties.WindowWidth / 2, 
                                                 WindowProperties.WindowHeight * 10 / 11);
            var healthBarDimensions = new Vector2f(WindowProperties.WindowWidth / 1.3f,
                                                   WindowProperties.WindowHeight / 15);
            hpBar.SetHpBarPositon(healthBarPosition, healthBarDimensions);
            var actualAnimationFrame = ActualAnimationFrame();
            window.Draw(backgroundBossFrames[actualAnimationFrame]);
            window.Draw(bossFrames[actualAnimationFrame]);
            hpBar.Draw(window);
        }

        public void Move()
        {
            var movement = new Vector2f();

            if(movementDirection.X == Directions.Left)
            {
                movement += new Vector2f(-1.2f * WindowProperties.ScaleX,
                                             0 * WindowProperties.ScaleY);
            }
            if (movementDirection.X == Directions.Right)
            {
                movement += new Vector2f(1.2f * WindowProperties.ScaleX,
                                            0 * WindowProperties.ScaleY);
            }
            if (movementDirection.Y == Directions.Up)
            {
                movement += new Vector2f(0 * WindowProperties.ScaleX,
                                     -0.4f * WindowProperties.ScaleY);
            }
            if (movementDirection.Y == Directions.Down)
            {
                movement += new Vector2f(0 * WindowProperties.ScaleX,
                                      0.4f * WindowProperties.ScaleY);
            }

            foreach (var bossFrame in bossFrames)
                bossFrame.Position += movement;

            foreach (var backgroundBossFrame in backgroundBossFrames)
                backgroundBossFrame.Position += movement;

            howMuchBossFliedInOneDirection.X++;
            howMuchBossFliedInOneDirection.Y++;

            CheckIfBossHasFliedEnough();
        }

        private void CheckIfBossHasFliedEnough()
        {
            if (howMuchBossFliedInOneDirection.X > WindowProperties.WindowWidth / 2)
            {
                movementDirection.X = movementDirection.X == Directions.Left ? Directions.Right : Directions.Left;

                howMuchBossFliedInOneDirection.X = 0;
            }
            if (!(howMuchBossFliedInOneDirection.Y > WindowProperties.WindowHeight / 4)) return;

            movementDirection.Y = movementDirection.Y == Directions.Up ? Directions.Down : Directions.Up;

            howMuchBossFliedInOneDirection.Y = 0;
        }

        public void Shoot(List<Bullet> enemiesBullets)
        {
            if ((int)bossClock.ElapsedTime.AsSeconds() % 7 == 0)
            {
                if (changingAttackSound.Status != SoundStatus.Playing)
                    changingAttackSound.Play();

                foreach (var bossFrame in bossFrames)
                {
                    bossFrame.Position = backgroundBossFrames[0].Position;
                    bossFrame.Color = Color.Red;
                }
                var newShootingMethod = shootingMethod;
                while (newShootingMethod == shootingMethod)
                    newShootingMethod = random.Next(0, 6);
                shootingMethod = newShootingMethod;
            }
            else
            {
                switch (shootingMethod)
                {
                    case 0:
                        ShootBulletsFromThinTwister(enemiesBullets);
                        break;
                    case 1:
                        ShootBulletsFromFatTwister(enemiesBullets);
                        break;
                    case 2:
                        ShootRandomBulletsBellow(enemiesBullets);
                        break;
                    case 3:
                        ShootBulletsToSides(enemiesBullets);
                        break;
                    case 4:
                        ShootArchsInRandomDirection(enemiesBullets);
                        break;
                    case 5:
                        ShootBulletsInCirclePatternFromAbove(enemiesBullets);
                        break;
                    default:
                        break;
                }

                foreach (var bossFrame in bossFrames)
                {
                    bossFrame.Color = Color.White;
                }
            }
        }

        private void ShootRandomBulletsBellow(List<Bullet> enemiesBullets)
        {
            if (reloadingClock.ElapsedTime.AsMilliseconds() > 200)
            {
                for (var i = 0; i <= 8; i++)
                {
                    var vector = new Vector2f(random.Next(-6, 6), random.Next(2, 4));
                    vector.X *= WindowProperties.ScaleX;
                    vector.Y *= WindowProperties.ScaleY;
                    enemiesBullets.Add(new Bullet(Position, vector));
                }

                reloadingClock.Restart();
            }
        }

        private void ShootBulletsToSides(List<Bullet> enemiesBullets)
        {
            if (reloadingClock.ElapsedTime.AsMilliseconds() > 20)
            {
                for (var x = 1.5f; x < 5; x += 0.4f)
                {
                    var y = (float)(Math.Cos(bossClock.ElapsedTime.AsMilliseconds() % 10000.0 / 700 * Math.PI * 4) * 3);

                    var centerOfTheScreen = new Vector2f(WindowProperties.WindowWidth / 2, WindowProperties.WindowHeight / 2);

                    enemiesBullets.Add(new Bullet(centerOfTheScreen, new Vector2f(x, y)));
                    enemiesBullets.Add(new Bullet(centerOfTheScreen, new Vector2f(-x, -y)));
                }
                reloadingClock.Restart();
            }
            SetBossInCenter();
        }

        private void ShootBulletsFromThinTwister(List<Bullet> enemiesBullets)
        {
            if (reloadingClock.ElapsedTime.AsMilliseconds() > 20)
            {
                for (var i = bossClock.ElapsedTime.AsMilliseconds() - 5000; i < bossClock.ElapsedTime.AsMilliseconds() + 5000; i += 1000)
                {
                    var x = (float)(Math.Sin(i % 10000.0 / 10000 * Math.PI * 4) * 4);
                    var y = (float)(Math.Cos(i % 10000.0 / 10000 * Math.PI * 4) * 4);

                    var centerOfTheScreen = new Vector2f(WindowProperties.WindowWidth / 2, WindowProperties.WindowHeight / 2);

                    enemiesBullets.Add(new Bullet(centerOfTheScreen, new Vector2f(x, y)));
                }
                reloadingClock.Restart();
            }
            SetBossInCenter();
        }

        private void ShootBulletsFromFatTwister(List<Bullet> enemiesBullets)
        {
            if (reloadingClock.ElapsedTime.AsMilliseconds() > 30)
            {
                for (var i = bossClock.ElapsedTime.AsMilliseconds() - 400; i < bossClock.ElapsedTime.AsMilliseconds() + 400; i += 80)
                {
                    var x = (float)(Math.Sin(i % 10000.0 / 10000 * Math.PI * 4) * 4);
                    var y = (float)(Math.Cos(i % 10000.0 / 10000 * Math.PI * 4) * 4);

                    var centerOfTheScreen = new Vector2f(WindowProperties.WindowWidth / 2, WindowProperties.WindowHeight / 2);

                    enemiesBullets.Add(new Bullet(centerOfTheScreen, new Vector2f(x, y)));
                    enemiesBullets.Add(new Bullet(centerOfTheScreen, new Vector2f(-x, -y)));
                }
                reloadingClock.Restart();
            }
            SetBossInCenter();
        }

        private void ShootBulletsInCirclePatternFromAbove(List<Bullet> enemiesBullets)
        {
            if (reloadingClock.ElapsedTime.AsMilliseconds() > 300)
            {
                for (var i = bossClock.ElapsedTime.AsMilliseconds() - 3000; i < bossClock.ElapsedTime.AsMilliseconds() + 3000; i += 500)
                {
                    var x = (float)(Math.Sin(i % 10000.0 / 10000 * Math.PI * 4) * 3);
                    var y = (float)(Math.Cos(i % 10000.0 / 10000 * Math.PI * 4) * 3);

                    enemiesBullets.Add(new Bullet(Position, new Vector2f(x, y)));
                }
                reloadingClock.Restart();
            }
        }

        private void ShootArchsInRandomDirection(List<Bullet> enemiesBullets)
        {
            if (reloadingClock.ElapsedTime.AsMilliseconds() > 300)
            {
                var randomDirection = random.Next(10000);
                for (var i = randomDirection - 500; i < randomDirection + 500; i += 40)
                {
                    var x = (float)(Math.Sin(i % 10000.0 / 10000 * Math.PI * 4) * 4);
                    var y = (float)(Math.Cos(i % 10000.0 / 10000 * Math.PI * 4) * 4);

                    var centerOfTheScreen = new Vector2f(WindowProperties.WindowWidth / 2, WindowProperties.WindowHeight / 2);

                    enemiesBullets.Add(new Bullet(centerOfTheScreen, new Vector2f(x, y)));
                }
                reloadingClock.Restart();
            }
            SetBossInCenter();
        }

        private void SetBossInCenter()
        {
            foreach (var bossFrame in bossFrames)
                bossFrame.Position = new Vector2f(WindowProperties.WindowWidth / 2, WindowProperties.WindowHeight / 2);
        }
    }
}
