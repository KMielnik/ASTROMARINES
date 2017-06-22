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
    internal class Level2 : ILevel
    {
        private readonly Texture _backgroundTexture;
        private readonly Sprite _background;
        private readonly IPlayer _player;
        private readonly IEnemyFactory _enemyFactory;
        private readonly List<IEnemy> _enemies;
        private readonly ExplosionFactory _explosionFactory;
        private readonly List<Explosion> _explosions;
        private readonly Clock _levelClock;
        private readonly Music _backgroundMusic;
        private readonly MousePointer _mousePointer;

        public bool HasLevelEnded { get; private set; }

        public Level2(IPlayer player)
        {
            _backgroundTexture = new Texture(Resources.LevelBG);
            _background = new Sprite(_backgroundTexture);
            _background.Scale = new Vector2f(WindowProperties.ScaleX, WindowProperties.ScaleY);

            _player = player;

            _mousePointer = new MousePointer();

            _enemyFactory = new EnemyFactory();
            _enemies = new List<IEnemy>();
            _explosionFactory = new ExplosionFactory();
            _explosions = new List<Explosion>();
            _levelClock = new Clock();

            _backgroundMusic = new Music(Resources.LevelBGMusic);
            _backgroundMusic.Loop = true;
            _backgroundMusic.Play();
        }

        public void Dispose()
        {
            HasLevelEnded = true;

            _backgroundTexture.Dispose();
            _background.Dispose();
            _enemyFactory.Dispose();
            foreach (var enemy in _enemies)
                enemy.Dispose();
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
            foreach (var enemy in _enemies)
                enemy.Draw(window);
            _player.Draw(window);
            foreach (var explosion in _explosions)
                explosion.Draw(window);
            _mousePointer.Draw(window);

            window.Display();
        }

        public void LevelLogic(ref RenderWindow window)
        {
            TryToCreateNewEnemy();
            ControlPlayer(window);
            MoveThoseWhoShallBeMoved();
            CheckDamage();
            LevelTime();
            DeleteObjectsSetForDeletion();
        }

        private void LevelTime()
        {
            var levelLength = Time.FromSeconds(7);
            var timeLeft = levelLength - _levelClock.ElapsedTime;
            var seconds = (int)timeLeft.AsSeconds();
            if (seconds < 0)
            {
                foreach (var enemy in _enemies)
                    enemy.ShouldBeDeleted = true;
            }
            if (seconds <= -2)
            {
                HasLevelEnded = true;
                _backgroundMusic.Stop();
            }
        }

        private void MoveThoseWhoShallBeMoved()
        {
            _player.Move();

            foreach (var enemy in _enemies)
            {
                enemy.Move();
            }
        }

        private void DeleteObjectsSetForDeletion()
        {
            for (var i = 0; i < _enemies.Count; i++)
                if (_enemies[i].ShouldBeDeleted)
                {
                    var newExplosion = _explosionFactory.CreateExplosion(_enemies[i].Position);
                    _explosions.Add(newExplosion);

                    _enemies[i].Dispose();
                    _enemies[i] = null;
                    _enemies.RemoveAt(i);
                }
            for (var i = 0; i < _explosions.Count; i++)
                if (_explosions[i].ShouldBeDeleted)
                {
                    _explosions[i].Dispose();
                    _explosions[i] = null;
                    _explosions.RemoveAt(i);
                }
        }

        private void CheckDamage()
        {
            foreach (var enemy in _enemies)
            {
                if (_player.BoundingBox.Intersects(enemy.BoudingBox))
                    _player.Damaged();

                foreach (var playerBullet in _player.Bullets)
                    if (enemy.BoudingBox.Contains(playerBullet.Position.X, playerBullet.Position.Y))
                    {
                        if (enemy.GetType() == typeof(Enemy1))
                            _player.LevelUp();
                        enemy.Damaged();
                        playerBullet.ShouldBeDeleted = true;
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

        private int _howMuchEnemiesToCreate;

        private void TryToCreateNewEnemy()
        {
            if (_howMuchEnemiesToCreate < 40)
            {
                if (_enemyFactory.IsNewEnemyAvalible())
                {
                    _enemies.Add(_enemyFactory.CreateEnemy(EnemyTypes.PowerUp));
                    _howMuchEnemiesToCreate++;
                }
            }
        }
    }
}