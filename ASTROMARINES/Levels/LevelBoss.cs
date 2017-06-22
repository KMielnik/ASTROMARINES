using System;
using System.Collections.Generic;
using ASTROMARINES.Characters.Enemies;
using ASTROMARINES.Characters.Player;
using ASTROMARINES.Other;
using ASTROMARINES.Properties;
using SFML.Audio;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace ASTROMARINES.Levels
{
    internal class LevelBoss : ILevel
    {
        private readonly Texture _backgroundTexture;
        private readonly Sprite _background;
        private readonly IPlayer _player;
        private readonly IEnemyFactory _enemyFactory;
        private readonly IEnemy _boss;
        private readonly List<Bullet> _bossBullets;
        private readonly ExplosionFactory _explosionFactory;
        private readonly List<Explosion> _explosions;
        private readonly Clock _levelClock;
        private readonly Music _backgroundMusic;
        private readonly MousePointer _mousePointer;
        private readonly Music _bossDeathSound;

        private bool _hasBossDied;
        private bool _hasLevelEnded;

        public bool HasLevelEnded
        {
            get => (_hasBossDied && _levelClock.ElapsedTime.AsSeconds() > 5) || _hasLevelEnded;
            private set => _hasLevelEnded = value;
        }

        public LevelBoss(IPlayer player)
        {
            _backgroundTexture = new Texture(Resources.LevelBG);
            _background = new Sprite(_backgroundTexture);
            _background.Scale = new Vector2f(WindowProperties.ScaleX, WindowProperties.ScaleY);

            _player = player;

            _mousePointer = new MousePointer();

            _enemyFactory = new EnemyFactory();
            _boss = _enemyFactory.CreateEnemy(EnemyTypes.Boss);
            _bossBullets = new List<Bullet>();
            _explosionFactory = new ExplosionFactory();
            _explosions = new List<Explosion>();
            _levelClock = new Clock();

            _backgroundMusic = new Music(Resources.BossBGMusic);
            _backgroundMusic.Loop = true;
            _backgroundMusic.Play();

            _bossDeathSound = new Music(Resources.ExplosionSound);
        }

        public void Dispose()
        {
            HasLevelEnded = true;

            _backgroundTexture.Dispose();
            _background.Dispose();
            _enemyFactory.Dispose();
            _boss.Dispose();
            _explosionFactory.Dispose();
            _levelClock.Dispose();
            _mousePointer.Dispose();
            _backgroundMusic.Stop();
            _backgroundMusic.Dispose();
        }

        public void Draw(RenderWindow window)
        {
            window.Clear(Color.Black);

            window.Draw(_background);
            if (_hasBossDied == false)
                _boss.Draw(window);
            foreach (var bullet in _bossBullets)
                bullet.Draw(window, Color.Cyan);
            _player.Draw(window);
            foreach (var explosion in _explosions)
                explosion.Draw(window);
            _mousePointer.Draw(window);

            window.Display();
        }

        public void LevelLogic(ref RenderWindow window)
        {
            ControlPlayer(window);
            MoveThoseWhoShallBeMoved();
            CheckDamage();
            DeleteObjectsSetForDeletion();
            CreateExplosionsIfBossDied();
        }

        private void MoveThoseWhoShallBeMoved()
        {
            _player.Move();

            if (_hasBossDied == false)
            {
                _boss.Move();
                _boss.Shoot(_bossBullets);
            }

            foreach (var bullet in _bossBullets)
            {
                bullet.Move();
            }
        }

        private void DeleteObjectsSetForDeletion()
        {
            if (_boss.ShouldBeDeleted)
            {
                if (_hasBossDied == false)
                {
                    _hasBossDied = true;
                    _levelClock.Restart();
                    _bossDeathSound.Play();
                }
            }
            for (var i = 0; i < _bossBullets.Count; i++)
                if (_bossBullets[i].ShouldBeDeleted)
                {
                    _bossBullets[i].Dispose();
                    _bossBullets[i] = null;
                    _bossBullets.RemoveAt(i);
                }
            for (var i = 0; i < _explosions.Count; i++)
                if (_explosions[i].ShouldBeDeleted)
                {
                    _explosions[i].Dispose();
                    _explosions[i] = null;
                    _explosions.RemoveAt(i);
                }
        }

        private void CreateExplosionsIfBossDied()
        {
            if(_hasBossDied)
                if(_levelClock.ElapsedTime.AsSeconds() < 2)
                {
                    if (_levelClock.ElapsedTime.AsMilliseconds() % 5 == 0)
                    {
                        var random = new Random();
                        var randXInBoss = random.Next((int)(_boss.BoudingBox.Left),
                                                      (int)(_boss.BoudingBox.Left + _boss.BoudingBox.Width * 1.5f));
                        var randYInBoss = random.Next((int)(_boss.BoudingBox.Top),
                                                      (int)(_boss.BoudingBox.Top + _boss.BoudingBox.Height));
                        var explosion = _explosionFactory.CreateExplosion(new Vector2f(randXInBoss, randYInBoss));
                        explosion.SetExplosionScale(random.Next(1, 5));

                        _explosions.Add(explosion);
                    }
                }
        }

        private void CheckDamage()
        {
            if (_boss.ShouldBeDeleted == false)
            {
                if (_player.BoundingBox.Intersects(_boss.BoudingBox))
                    _player.Damaged();

                foreach (var playerBullet in _player.Bullets)
                    if (_boss.BoudingBox.Contains(playerBullet.Position.X, playerBullet.Position.Y))
                    {
                        _boss.Damaged();
                        playerBullet.ShouldBeDeleted = true;
                        _explosions.Add(_explosionFactory.CreateExplosion(playerBullet.Position));
                    }
            }

            foreach (var bossBullet in _bossBullets)
            {
                if (_player.BoundingBox.Contains(bossBullet.Position.X, bossBullet.Position.Y))
                {
                    _player.Damaged();
                    bossBullet.ShouldBeDeleted = true;
                }
            }
        }

        private void ControlPlayer(RenderWindow window)
        {
            if (Keyboard.IsKeyPressed(Keyboard.Key.A))
                _player.AccelerateLeft();
            if (Keyboard.IsKeyPressed(Keyboard.Key.D))
                _player.AccelerateRight();
            if (Keyboard.IsKeyPressed(Keyboard.Key.W))
                _player.AccelerateUp();
            if (Keyboard.IsKeyPressed(Keyboard.Key.S))
                _player.AccelerateDown();
            if (Keyboard.IsKeyPressed(Keyboard.Key.Space))
                _player.Shoot(window);
        }
    }
}