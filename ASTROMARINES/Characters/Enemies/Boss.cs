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

        private DirectionsVector _movementDirection;
        private Vector2f _howMuchBossFliedInOneDirection;
        private float _hp;
        private readonly float _hpMax;
        // ReSharper disable once NotAccessedField.Local
        private Vector2f _dimensions;
        private readonly List<Sprite> _bossFrames;
        private readonly List<Sprite> _backgroundBossFrames;
        private readonly HpBar _hpBar;
        private readonly Clock _reloadingClock;
        private readonly Clock _animationClock;
        private readonly Clock _bossClock;
        private readonly Random _random;
        private readonly Music _changingAttackSound;

        private int _shootingMethod;

        private bool _shouldBeDeleted;

        public Boss(Texture bossTexture, Texture backgroundBossTexture)
        {
            _dimensions = new Vector2f
            {
                X = 512 * 0.7f * WindowProperties.ScaleX,
                Y = 204 * 0.7f * WindowProperties.ScaleY
            };

            _bossFrames = new List<Sprite>();
            _backgroundBossFrames = new List<Sprite>();

            for (var i = 0; i < 6; i++)
            {
                {
                    var enemyFrame = new Sprite(bossTexture);
                    enemyFrame.Origin = new Vector2f(256f, 192f);
                    enemyFrame.Scale = new Vector2f(0.7f * WindowProperties.ScaleX,
                                                    0.7f * WindowProperties.ScaleY);
                    enemyFrame.Position = new Vector2f(WindowProperties.WindowWidth / 2, WindowProperties.WindowHeight / 7);
                    enemyFrame.TextureRect = new IntRect(i * 512, 0, 512, 204);

                    _bossFrames.Add(enemyFrame);
                }

                {
                    var backgroundEnemyFrame = new Sprite(backgroundBossTexture);
                    backgroundEnemyFrame.Origin = new Vector2f(500f, 104f);
                    backgroundEnemyFrame.Scale = new Vector2f(0.7f * WindowProperties.ScaleX,
                                                              0.7f * WindowProperties.ScaleY);
                    backgroundEnemyFrame.Position = new Vector2f(WindowProperties.WindowWidth / 2, WindowProperties.WindowHeight / 7);
                    backgroundEnemyFrame.TextureRect = new IntRect(i * 1000, 0, 1000, 208);

                    _backgroundBossFrames.Add(backgroundEnemyFrame);
                }
            }
            _movementDirection = new DirectionsVector(Directions.Left,
                                                     Directions.Down);

            _hpBar = new HpBar(new Vector2f(WindowProperties.WindowWidth / 1.3f,
                                           WindowProperties.WindowHeight / 15));

            _changingAttackSound = new Music(Resources.BossWarpSound);
            _changingAttackSound.Stop();

            _reloadingClock = new Clock();
            _animationClock = new Clock();
            _bossClock = new Clock();

            _random = new Random();

            _howMuchBossFliedInOneDirection = new Vector2f(WindowProperties.WindowWidth / 4,
                                                          WindowProperties.WindowHeight / 8);

            _hpMax = 3000;
            _hp = _hpMax;
        }

        public bool ShouldBeDeleted
        {
            get
            {
                if (_shouldBeDeleted)
                    return true;
                if (_hp <= 0)
                    return true;
                return false;
            }
            set
            {
                if (value)
                    _shouldBeDeleted = true;
            }
        }

        protected int ActualAnimationFrame()
        {
            var timeFromLastAnimationRestart = _animationClock.ElapsedTime.AsSeconds();
            var actualAnimationFrame = (int)(timeFromLastAnimationRestart * 5);                //new frame every 0.1 second
            if (actualAnimationFrame >= _bossFrames.Count)
            {
                actualAnimationFrame = 0;
                _animationClock.Restart();
            }
            return actualAnimationFrame;
        }

        public Vector2f Position => _bossFrames[0].Position;

        public FloatRect BoudingBox => _bossFrames[0].GetGlobalBounds();

        public void Damaged()
        {
            _hp--;
            _hpBar.UpdateHpBarSize(_hp, _hpMax);
        }

        public void Dispose()
        {
            foreach (var bossFrame in _bossFrames)
                bossFrame.Dispose();
            foreach (var backgroundBossFrame in _backgroundBossFrames)
                backgroundBossFrame.Dispose();

            _changingAttackSound.Dispose();

            _hpBar.Dispose();
            _reloadingClock.Dispose();
            _animationClock.Dispose();
        }

        public virtual void Draw(RenderWindow window)
        {
            var healthBarPosition = new Vector2f(WindowProperties.WindowWidth / 2, 
                                                 WindowProperties.WindowHeight * 10 / 11);
            var healthBarDimensions = new Vector2f(WindowProperties.WindowWidth / 1.3f,
                                                   WindowProperties.WindowHeight / 15);
            _hpBar.SetHpBarPositon(healthBarPosition, healthBarDimensions);
            var actualAnimationFrame = ActualAnimationFrame();
            window.Draw(_backgroundBossFrames[actualAnimationFrame]);
            window.Draw(_bossFrames[actualAnimationFrame]);
            _hpBar.Draw(window);
        }

        public void Move()
        {
            var movement = new Vector2f();

            if(_movementDirection.X == Directions.Left)
            {
                movement += new Vector2f(-1.2f * WindowProperties.ScaleX,
                                             0 * WindowProperties.ScaleY);
            }
            if (_movementDirection.X == Directions.Right)
            {
                movement += new Vector2f(1.2f * WindowProperties.ScaleX,
                                            0 * WindowProperties.ScaleY);
            }
            if (_movementDirection.Y == Directions.Up)
            {
                movement += new Vector2f(0 * WindowProperties.ScaleX,
                                     -0.4f * WindowProperties.ScaleY);
            }
            if (_movementDirection.Y == Directions.Down)
            {
                movement += new Vector2f(0 * WindowProperties.ScaleX,
                                      0.4f * WindowProperties.ScaleY);
            }

            foreach (var bossFrame in _bossFrames)
                bossFrame.Position += movement;

            foreach (var backgroundBossFrame in _backgroundBossFrames)
                backgroundBossFrame.Position += movement;

            _howMuchBossFliedInOneDirection.X++;
            _howMuchBossFliedInOneDirection.Y++;

            CheckIfBossHasFliedEnough();
        }

        private void CheckIfBossHasFliedEnough()
        {
            if (_howMuchBossFliedInOneDirection.X > WindowProperties.WindowWidth / 2)
            {
                if (_movementDirection.X == Directions.Left)
                    _movementDirection.X = Directions.Right;
                else
                    _movementDirection.X = Directions.Left;

                _howMuchBossFliedInOneDirection.X = 0;
            }
            if (_howMuchBossFliedInOneDirection.Y > WindowProperties.WindowHeight / 4)
            {

                if (_movementDirection.Y == Directions.Up)
                    _movementDirection.Y = Directions.Down;
                else
                    _movementDirection.Y = Directions.Up;

                _howMuchBossFliedInOneDirection.Y = 0;
            }
        }

        public void Shoot(List<Bullet> enemiesBullets)
        {
            if ((int)_bossClock.ElapsedTime.AsSeconds() % 7 == 0)
            {
                if (_changingAttackSound.Status != SoundStatus.Playing)
                    _changingAttackSound.Play();

                foreach (var bossFrame in _bossFrames)
                {
                    bossFrame.Position = _backgroundBossFrames[0].Position;
                    bossFrame.Color = Color.Red;
                }
                var newShootingMethod = _shootingMethod;
                while (newShootingMethod == _shootingMethod)
                    newShootingMethod = _random.Next(0, 6);
                _shootingMethod = newShootingMethod;
            }
            else
            {
                switch (_shootingMethod)
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
                }

                foreach (var bossFrame in _bossFrames)
                {
                    bossFrame.Color = Color.White;
                }
            }
        }
        
        private void ShootRandomBulletsBellow(List<Bullet> enemiesBullets)
        {
            if (_reloadingClock.ElapsedTime.AsMilliseconds() > 200)
            {
                for (var i = 0; i <= 8; i++)
                {
                    var vector = new Vector2f(_random.Next(-6, 6), _random.Next(2, 4));
                    vector.X *= WindowProperties.ScaleX;
                    vector.Y *= WindowProperties.ScaleY;
                    enemiesBullets.Add(new Bullet(Position, vector));
                }

                _reloadingClock.Restart();
            }
        }

        private void ShootBulletsToSides(List<Bullet> enemiesBullets)
        {
            if (_reloadingClock.ElapsedTime.AsMilliseconds() > 20)
            {
                for (var x = 1.5f; x < 5; x += 0.4f)
                {
                    var y = (float)(Math.Cos(_bossClock.ElapsedTime.AsMilliseconds() % 10000.0 / 700 * Math.PI * 4) * 3);

                    var centerOfTheScreen = new Vector2f(WindowProperties.WindowWidth / 2, WindowProperties.WindowHeight / 2);

                    enemiesBullets.Add(new Bullet(centerOfTheScreen, new Vector2f(x, y)));
                    enemiesBullets.Add(new Bullet(centerOfTheScreen, new Vector2f(-x, -y)));
                }
                _reloadingClock.Restart();
            }
            SetBossInCenter();
        }

        private void ShootBulletsFromThinTwister(List<Bullet> enemiesBullets)
        {
            if (_reloadingClock.ElapsedTime.AsMilliseconds() > 20)
            {
                for (var i = _bossClock.ElapsedTime.AsMilliseconds() - 5000; i < _bossClock.ElapsedTime.AsMilliseconds() + 5000; i += 1000)
                {
                    var x = (float)(Math.Sin(i % 10000.0 / 10000 * Math.PI * 4) * 4);
                    var y = (float)(Math.Cos(i % 10000.0 / 10000 * Math.PI * 4) * 4);

                    var centerOfTheScreen = new Vector2f(WindowProperties.WindowWidth / 2, WindowProperties.WindowHeight / 2);

                    enemiesBullets.Add(new Bullet(centerOfTheScreen, new Vector2f(x, y)));
                }
                _reloadingClock.Restart();
            }
            SetBossInCenter();
        }

        private void ShootBulletsFromFatTwister(List<Bullet> enemiesBullets)
        {
            if (_reloadingClock.ElapsedTime.AsMilliseconds() > 30)
            {
                for (var i = _bossClock.ElapsedTime.AsMilliseconds() - 400; i < _bossClock.ElapsedTime.AsMilliseconds() + 400; i += 80)
                {
                    var x = (float)(Math.Sin(i % 10000.0 / 10000 * Math.PI * 4) * 4);
                    var y = (float)(Math.Cos(i % 10000.0 / 10000 * Math.PI * 4) * 4);

                    var centerOfTheScreen = new Vector2f(WindowProperties.WindowWidth / 2, WindowProperties.WindowHeight / 2);

                    enemiesBullets.Add(new Bullet(centerOfTheScreen, new Vector2f(x, y)));
                    enemiesBullets.Add(new Bullet(centerOfTheScreen, new Vector2f(-x, -y)));
                }
                _reloadingClock.Restart();
            }
            SetBossInCenter();
        }

        private void ShootBulletsInCirclePatternFromAbove(List<Bullet> enemiesBullets)
        {
            if (_reloadingClock.ElapsedTime.AsMilliseconds() > 300)
            {
                for (var i = _bossClock.ElapsedTime.AsMilliseconds() - 3000; i < _bossClock.ElapsedTime.AsMilliseconds() + 3000; i += 500)
                {
                    var x = (float)(Math.Sin(i % 10000.0 / 10000 * Math.PI * 4) * 3);
                    var y = (float)(Math.Cos(i % 10000.0 / 10000 * Math.PI * 4) * 3);

                    enemiesBullets.Add(new Bullet(Position, new Vector2f(x, y)));
                }
                _reloadingClock.Restart();
            }
        }

        private void ShootArchsInRandomDirection(List<Bullet> enemiesBullets)
        {
            if (_reloadingClock.ElapsedTime.AsMilliseconds() > 300)
            {
                var randomDirection = _random.Next(10000);
                for (var i = randomDirection - 500; i < randomDirection + 500; i += 40)
                {
                    var x = (float)(Math.Sin(i % 10000.0 / 10000 * Math.PI * 4) * 4);
                    var y = (float)(Math.Cos(i % 10000.0 / 10000 * Math.PI * 4) * 4);

                    var centerOfTheScreen = new Vector2f(WindowProperties.WindowWidth / 2, WindowProperties.WindowHeight / 2);

                    enemiesBullets.Add(new Bullet(centerOfTheScreen, new Vector2f(x, y)));
                }
                _reloadingClock.Restart();
            }
            SetBossInCenter();
        }

        private void SetBossInCenter()
        {
            foreach (var bossFrame in _bossFrames)
                bossFrame.Position = new Vector2f(WindowProperties.WindowWidth / 2, WindowProperties.WindowHeight / 2);
        }
    }
}
