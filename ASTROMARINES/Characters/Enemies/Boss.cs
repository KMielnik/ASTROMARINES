using System;
using System.Collections.Generic;
using ASTROMARINES.Other;
using ASTROMARINES.Properties;
using SFML.Audio;
using SFML.Graphics;
using SFML.System;

namespace ASTROMARINES.Characters.Enemies
{
    internal class Boss : IEnemy
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
        private float hp;
        private float hpMax;
        private Vector2f dimensions;
        private List<Sprite> bossFrames;
        private List<Sprite> backgroundBossFrames;
        private HpBar hpBar;
        private Clock reloadingClock;
        private Clock animationClock;
        private Clock bossClock;
        private Random random;
        private Music changingAttackSound;

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
                    var enemyFrame = new Sprite(bossTexture);
                    enemyFrame.Origin = new Vector2f(256f, 192f);
                    enemyFrame.Scale = new Vector2f(0.7f * WindowProperties.ScaleX,
                                                    0.7f * WindowProperties.ScaleY);
                    enemyFrame.Position = new Vector2f(WindowProperties.WindowWidth / 2, WindowProperties.WindowHeight / 7);
                    enemyFrame.TextureRect = new IntRect(i * 512, 0, 512, 204);

                    bossFrames.Add(enemyFrame);
                }

                {
                    var backgroundEnemyFrame = new Sprite(backgroundBossTexture);
                    backgroundEnemyFrame.Origin = new Vector2f(500f, 104f);
                    backgroundEnemyFrame.Scale = new Vector2f(0.7f * WindowProperties.ScaleX,
                                                              0.7f * WindowProperties.ScaleY);
                    backgroundEnemyFrame.Position = new Vector2f(WindowProperties.WindowWidth / 2, WindowProperties.WindowHeight / 7);
                    backgroundEnemyFrame.TextureRect = new IntRect(i * 1000, 0, 1000, 208);

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
            hp = hpMax;
        }

        public bool ShouldBeDeleted
        {
            get
            {
                if (shouldBeDeleted)
                    return true;
                if (hp <= 0)
                    return true;
                return false;
            }
            set
            {
                if (value)
                    shouldBeDeleted = true;
            }
        }

        protected int ActualAnimationFrame()
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

        public void Damaged()
        {
            hp--;
            hpBar.UpdateHpBarSize(hp, hpMax);
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

        public virtual void Draw(RenderWindow window)
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
                if (movementDirection.X == Directions.Left)
                    movementDirection.X = Directions.Right;
                else
                    movementDirection.X = Directions.Left;

                howMuchBossFliedInOneDirection.X = 0;
            }
            if (howMuchBossFliedInOneDirection.Y > WindowProperties.WindowHeight / 4)
            {

                if (movementDirection.Y == Directions.Up)
                    movementDirection.Y = Directions.Down;
                else
                    movementDirection.Y = Directions.Up;

                howMuchBossFliedInOneDirection.Y = 0;
            }
        }

        public void Shoot(List<Bullet> enemiesBullets)
        {
            if ((int)bossClock.ElapsedTime.AsSeconds() % 7 == 0)
            {
                if ((int)bossClock.ElapsedTime.AsSeconds() != 0)
                    if (changingAttackSound.Status != SoundStatus.Playing)
                        changingAttackSound.Play();

                foreach (var bossFrame in bossFrames)
                    bossFrame.Position = backgroundBossFrames[0].Position;

                var newShootingMethod = shootingMethod;
                while (newShootingMethod == shootingMethod)
                    newShootingMethod = random.Next(0, 2);
                shootingMethod = newShootingMethod;
            }
            else
            {
                switch (shootingMethod)
                {
                    case 0:
                        ShootBulletsFromTwister(enemiesBullets);
                        break;
                    case 1:
                        ShootRandomBulletsBellow(enemiesBullets);
                        break;
                    case 2:
                        break;
                    case 3:
                        break;
                }
            }



        }

        private void ShootBulletsFromTwister(List<Bullet> enemiesBullets)
        {
            if (reloadingClock.ElapsedTime.AsMilliseconds() > 20)
            {

                var x = (float)(Math.Sin(bossClock.ElapsedTime.AsMilliseconds() % 10000.0 / 10000 * Math.PI * 4) * 4);
                var y = (float)(Math.Cos(bossClock.ElapsedTime.AsMilliseconds() % 10000.0 / 10000 * Math.PI * 4) * 4);

                var centerOfTheScreen = new Vector2f(WindowProperties.WindowWidth / 2, WindowProperties.WindowHeight / 2);

                enemiesBullets.Add(new Bullet(centerOfTheScreen, new Vector2f(x, y)));
                enemiesBullets.Add(new Bullet(centerOfTheScreen, new Vector2f(-x, -y)));

                reloadingClock.Restart();
            }

            foreach (var bossFrame in bossFrames)
                bossFrame.Position = new Vector2f(WindowProperties.WindowWidth / 2, WindowProperties.WindowHeight / 2);
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
    }
}
