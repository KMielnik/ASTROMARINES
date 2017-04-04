using System;
using SFML.Graphics;
using ASTROMARINES.Characters.Player;
using System.Collections.Generic;
using ASTROMARINES.Characters.Enemies;
using ASTROMARINES.Other;
using SFML.Audio;
using SFML.System;
using ASTROMARINES.Properties;
using SFML.Window;

namespace ASTROMARINES.Levels
{
    class Level1 : ILevel
    {
        Texture backgroundTexture;
        Sprite background;
        IPlayer player;
        IEnemyFactory enemyFactory;
        List<IEnemy> enemies;
        List<Bullet> enemyBullets;
        ExplosionFactory explosionFactory;
        List<Explosion> explosions;
        Clock levelClock;
        Font digitalClockFont;
        Text digitalClock;
        Music backgroundMusic;
        MousePointer mousePointer;
        bool hasLevelEnded;

        public bool HasLevelEnded { get => hasLevelEnded; private set => hasLevelEnded = value; }

        public Level1(IPlayer player)
        {
            backgroundTexture = new Texture(Resources.LevelBG);
            background = new Sprite(backgroundTexture);
            background.Scale = new Vector2f(WindowProperties.ScaleX, WindowProperties.ScaleY);

            this.player = player;

            mousePointer = new MousePointer();

            enemyFactory = new EnemyFactory();
            enemies = new List<IEnemy>();
            enemyBullets = new List<Bullet>();
            explosionFactory = new ExplosionFactory();
            explosions = new List<Explosion>();
            levelClock = new Clock();

            digitalClockFont = new Font(Resources.FontDigitalClock);
            digitalClock = new Text("", digitalClockFont, 30);
            digitalClock.Position = new Vector2f(WindowProperties.WindowWidth * 30 / 32, WindowProperties.WindowHeight * 30 / 32);
            digitalClock.Color = new Color(Color.White);

            backgroundMusic = new Music(Resources.LevelBGMusic);
            backgroundMusic.Loop = true;
            backgroundMusic.Play();
        }

        public void Dispose()
        {
            HasLevelEnded = true;

            backgroundTexture.Dispose();
            background.Dispose();
            enemyFactory.Dispose();
            foreach (var enemy in enemies)
                enemy.Dispose();
            explosionFactory.Dispose();
            levelClock.Dispose();
            digitalClockFont.Dispose();
            digitalClock.Dispose();
            mousePointer.Dispose();
            backgroundMusic.Stop();
            backgroundMusic.Dispose();
        }

        public void Draw(RenderWindow window)
        {
            window.Clear(Color.Black);

            window.Draw(background);
            foreach (var enemy in enemies)
                enemy.Draw(window);
            foreach (var bullet in enemyBullets)
                bullet.Draw(window, Color.Cyan);
            player.Draw(window);
            foreach (var explosion in explosions)
                explosion.Draw(window);
            window.Draw(digitalClock);
            mousePointer.Draw(window);

            window.Display();
        }

        public void LevelLogic(RenderWindow window)
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
            Time levelLength = Time.FromSeconds(60);
            Time timeLeft = levelLength - levelClock.ElapsedTime;
            int seconds = (int)timeLeft.AsSeconds();
            string miliseconds = (timeLeft.AsSeconds() - seconds).ToString();
            string timeToBeDisplayed = seconds.ToString() + '.' + miliseconds[2];
            digitalClock.DisplayedString = timeToBeDisplayed;
            if (seconds <= 0 && miliseconds[2]=='.')
            {
                digitalClock.DisplayedString = "END";
                foreach (var enemy in enemies)
                    enemy.ShouldBeDeleted = true;
                foreach (var enemyBullet in enemyBullets)
                    enemyBullet.ShouldBeDeleted = true;
            }
            if (seconds <= -5)
            {
                HasLevelEnded = true;
                backgroundMusic.Stop();
            }
        }

        private void MoveThoseWhoShallBeMoved()
        {
            player.Move();
            foreach (var enemy in enemies)
            {
                enemy.Move();
                enemy.Shoot(enemyBullets);
            }
            foreach (var bullet in enemyBullets)
            {
                bullet.Move();
            }
        }

        private void DeleteObjectsSetForDeletion()
        {
            for (int i = 0; i < enemies.Count; i++)
                if (enemies[i].ShouldBeDeleted)
                {
                    var newExplosion = explosionFactory.CreateExplosion(enemies[i].Position);
                    explosions.Add(newExplosion);

                    enemies[i].Dispose();
                    enemies[i] = null;
                    enemies.RemoveAt(i);
                }
            for (int i = 0; i < enemyBullets.Count; i++)
                if (enemyBullets[i].ShouldBeDeleted)
                {
                    enemyBullets[i].Dispose();
                    enemyBullets[i] = null;
                    enemyBullets.RemoveAt(i);
                }
            for (int i = 0; i < explosions.Count; i++)
                if (explosions[i].ShouldBeDeleted)
                {
                    explosions[i].Dispose();
                    explosions[i] = null;
                    explosions.RemoveAt(i);
                }
        }

        private void CheckDamage()
        {
            foreach (var enemy in enemies)
            {
                if (player.BoundingBox.Intersects(enemy.BoudingBox))
                    player.Damaged();

                foreach (var playerBullet in player.Bullets)
                    if (enemy.BoudingBox.Contains(playerBullet.Position.X, playerBullet.Position.Y))
                    {
                        if (enemy.GetType() == typeof(Enemy1))
                            player.LevelUp();
                        enemy.Damaged();
                        playerBullet.ShouldBeDeleted = true;
                    }
            }

            foreach(var enemyBullet in enemyBullets)
            {
                if(player.BoundingBox.Contains(enemyBullet.Position.X,enemyBullet.Position.Y))
                {
                    player.Damaged();
                    enemyBullet.ShouldBeDeleted = true;
                }
            }
        }

        private void ControlPlayer(RenderWindow window)
        {
            if (Keyboard.IsKeyPressed(Keyboard.Key.A))
                player.AccelerateLeft();
            if (Keyboard.IsKeyPressed(Keyboard.Key.D))
                player.AccelerateRight();
            if (Keyboard.IsKeyPressed(Keyboard.Key.W))
                player.AccelerateUp();
            if (Keyboard.IsKeyPressed(Keyboard.Key.S))
                player.AccelerateDown();
            if (Keyboard.IsKeyPressed(Keyboard.Key.Space))
                player.Shoot(window);
        }

        void TryToCreateNewEnemy()
        {
            if (enemyFactory.IsNewEnemyAvalible())
                enemies.Add(enemyFactory.CreateRandomEnemy());
            if (enemyFactory.IsPowerUpAvalible())
                enemies.Add(enemyFactory.CreateEnemy(EnemyTypes.PowerUp));
        }
    }
}