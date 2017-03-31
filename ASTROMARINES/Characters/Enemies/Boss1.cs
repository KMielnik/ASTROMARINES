using SFML.Graphics;
using SFML.System;
using System.Collections.Generic;
using ASTROMARINES.Other;
using System;

namespace ASTROMARINES.Characters.Enemies
{
    class Boss1 : IEnemy
    {
        Tuple<Directions, Directions> movementDirection;
        Vector2f howMuchBossFliedInOneDirection;
        float HP;
        float HPMax;
        Vector2f dimensions;
        List<Sprite> bossFrames;
        List<Sprite> backgroundBossFrames;
        HPBar hpBar;
        Clock reloadingClock;
        Clock animationClock;

        bool shouldBeDeleted;

        public Boss1(Texture bossTexture, Texture backgroundBossTexture)
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
                    enemyFrame.Position = new Vector2f(WindowProperties.WindowWidth / 2, WindowProperties.WindowHeight / 8);
                    enemyFrame.TextureRect = new IntRect(i * 512, 0, 512, 204);

                    bossFrames.Add(enemyFrame);
                }

                {
                    Sprite backgroundEnemyFrame = new Sprite(backgroundBossTexture);
                    backgroundEnemyFrame.Origin = new Vector2f(500f, 104f);
                    backgroundEnemyFrame.Scale = new Vector2f(0.7f * WindowProperties.ScaleX,
                                                              0.7f * WindowProperties.ScaleY);
                    backgroundEnemyFrame.Position = new Vector2f(WindowProperties.WindowWidth / 2, WindowProperties.WindowHeight / 8);
                    backgroundEnemyFrame.TextureRect = new IntRect(i * 1000, 0, 1000, 208);

                    backgroundBossFrames.Add(backgroundEnemyFrame);
                }

                movementDirection = new Tuple<Directions, Directions>(Directions.Left,
                                                                      Directions.Down);

                hpBar = new HPBar(new Vector2f(WindowProperties.WindowWidth / 1.3f,
                                               WindowProperties.WindowHeight / 20));
                reloadingClock = new Clock();
                animationClock = new Clock();

                HPMax = 200;
                HP = HPMax;
            }
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
            throw new NotImplementedException();
        }

        public virtual void Draw(RenderWindow window)
        {
            var healthBarPosition = new Vector2f(WindowProperties.WindowWidth / 2, 
                                                 WindowProperties.WindowHeight * 10 / 11);
            var healthBarDimensions = new Vector2f(WindowProperties.WindowWidth / 1.3f,
                                                   WindowProperties.WindowHeight / 20);
            hpBar.SetHPBarPositon(healthBarPosition, healthBarDimensions);
            var actualAnimationFrame = ActualAnimationFrame();
            window.Draw(backgroundBossFrames[actualAnimationFrame]);
            window.Draw(bossFrames[actualAnimationFrame]);
            hpBar.Draw(window);
        }

        public void Move()
        {
            throw new NotImplementedException();
        }

        public void Shoot(List<Bullet> EnemiesBullets)
        {
            throw new NotImplementedException();
        }
    }
}
