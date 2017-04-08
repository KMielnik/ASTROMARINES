using SFML.Graphics;
using SFML.System;
using System.Collections.Generic;
using ASTROMARINES.Other;
using System;
using SFML.Audio;
using ASTROMARINES.Properties;

namespace ASTROMARINES.Characters.Enemies
{
    class Boss : IEnemy
    {
        struct DirectionsVector
        {
            public Directions X;
            public Directions Y;
            public DirectionsVector(Directions x,Directions y)
            {
                this.X = x;
                this.Y = y;
            }
        }

        DirectionsVector movementDirection;
        Vector2f howMuchBossFliedInOneDirection;
        float HP;
        float HPMax;
        Vector2f dimensions;
        List<Sprite> bossFrames;
        List<Sprite> backgroundBossFrames;
        HPBar hpBar;
        Clock reloadingClock;
        Clock animationClock;
        Clock bossClock;
        Random random;
        Music changingAttackSound;

        int shootingMethod;

        bool shouldBeDeleted;

        public Boss(Texture bossTexture, Texture backgroundBossTexture)
        {
            dimensions.X = 512 * 0.7f * WindowProperties.ScaleX;
            dimensions.Y = 204 * 0.7f * WindowProperties.ScaleY;

            bossFrames = new List<Sprite>();
            backgroundBossFrames = new List<Sprite>();

            for (int i = 0; i < 6; i++)
            {
                {
                    Sprite enemyFrame = new Sprite(bossTexture);
                    enemyFrame.Origin = new Vector2f(256f, 192f);
                    enemyFrame.Scale = new Vector2f(0.7f * WindowProperties.ScaleX,
                                                    0.7f * WindowProperties.ScaleY);
                    enemyFrame.Position = new Vector2f(WindowProperties.WindowWidth / 2, WindowProperties.WindowHeight / 7);
                    enemyFrame.TextureRect = new IntRect(i * 512, 0, 512, 204);

                    bossFrames.Add(enemyFrame);
                }

                {
                    Sprite backgroundEnemyFrame = new Sprite(backgroundBossTexture);
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

            hpBar = new HPBar(new Vector2f(WindowProperties.WindowWidth / 1.3f,
                                           WindowProperties.WindowHeight / 15));

            changingAttackSound = new Music(Resources.BossWarpSound);
            changingAttackSound.Stop();

            reloadingClock = new Clock();
            animationClock = new Clock();
            bossClock = new Clock();

            random = new Random();

            howMuchBossFliedInOneDirection = new Vector2f(WindowProperties.WindowWidth / 4,
                                                          WindowProperties.WindowHeight / 8);

            HPMax = 3000;
            HP = HPMax;
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

        public Vector2f Position { get => bossFrames[0].Position; }

        public FloatRect BoudingBox { get => bossFrames[0].GetGlobalBounds(); }

        public void Damaged()
        {
            HP--;
            hpBar.UpdateHPBarSize(HP, HPMax);
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
            hpBar.SetHPBarPositon(healthBarPosition, healthBarDimensions);
            var actualAnimationFrame = ActualAnimationFrame();
            window.Draw(backgroundBossFrames[actualAnimationFrame]);
            window.Draw(bossFrames[actualAnimationFrame]);
            hpBar.Draw(window);
        }

        public void Move()
        {
            Vector2f movement = new Vector2f();

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

        public void Shoot(List<Bullet> EnemiesBullets)
        {
            if ((int)bossClock.ElapsedTime.AsSeconds() % 7 == 0)
            {
                if ((int)bossClock.ElapsedTime.AsSeconds() != 0)
                    if (changingAttackSound.Status != SoundStatus.Playing)
                        changingAttackSound.Play();

                foreach (var bossFrame in bossFrames)
                    bossFrame.Position = backgroundBossFrames[0].Position;

                int newShootingMethod = shootingMethod;
                while (newShootingMethod == shootingMethod)
                    newShootingMethod = random.Next(0, 2);
                shootingMethod = newShootingMethod;
            }
            else
            {
                switch (shootingMethod)
                {
                    case 0:
                        ShootBulletsFromTwister(EnemiesBullets);
                        break;
                    case 1:
                        ShootRandomBulletsBellow(EnemiesBullets);
                        break;
                    case 2:
                        break;
                    case 3:
                        break;
                    default:
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

        private void ShootRandomBulletsBellow(List<Bullet> EnemiesBullets)
        {
            if (reloadingClock.ElapsedTime.AsMilliseconds() > 200)
            {
                for (int i = 0; i <= 8; i++)
                {
                    var vector = new Vector2f(random.Next(-6, 6), random.Next(2, 4));
                    vector.X *= WindowProperties.ScaleX;
                    vector.Y *= WindowProperties.ScaleY;
                    EnemiesBullets.Add(new Bullet(Position, vector));
                }

                reloadingClock.Restart();
            }
        }
    }
}
