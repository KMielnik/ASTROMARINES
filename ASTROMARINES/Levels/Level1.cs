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

        public bool HasLevelEnded { get => levelClock.ElapsedTime.AsSeconds() > 65; }

        public Level1(IPlayer player)
        {
            backgroundTexture = new Texture(Resources.LevelBG);
            background = new Sprite(backgroundTexture);
            background.Scale = new Vector2f(WindowProperties.ScaleX, WindowProperties.ScaleY);

            this.player = player;

            enemyFactory = new EnemyFactory();
            enemies = new List<IEnemy>();
            enemyBullets = new List<Bullet>();
            explosionFactory = new ExplosionFactory();
            explosions = new List<Explosion>();
            levelClock = new Clock();

            digitalClockFont = new Font(Resources.FontDigitalClock);
            digitalClock = new Text("", digitalClockFont, 30 * (uint)WindowProperties.ScaleY);
            digitalClock.Position = new Vector2f(WindowProperties.ScaleX * 31 / 32, WindowProperties.ScaleY * 20 / 21);
            digitalClock.Color = new Color(Color.White);

            backgroundMusic = new Music(Resources.Level1BGMusic);
            backgroundMusic.Loop = true;
            backgroundMusic.Play();
        }

        public void Dispose()
        {
            player.LevelUp();

            backgroundTexture.Dispose();
            background.Dispose();
            enemyFactory.Dispose();
            foreach (var enemy in enemies)
                enemy.Dispose();
            explosionFactory.Dispose();
            //levelClock.Dispose();
            digitalClockFont.Dispose();
            digitalClock.Dispose();
            backgroundMusic.Dispose();
        }

        public void Draw(RenderWindow window)
        {
            window.Clear(Color.Black);

            window.Draw(background);
            foreach (var explosion in explosions)
                explosion.Draw(window);
            foreach (var enemy in enemies)
                enemy.Draw(window);
            foreach (var bullet in enemyBullets)
                bullet.Draw(window, Color.Cyan);
            player.DrawPlayer(window);
            window.Draw(digitalClock);

            window.Display();
        }

        public void LevelLogic(RenderWindow window)
        {
            TryToCreateNewEnemy();
            MovePlayer(window);
            player.Move();
            foreach (var enemy in enemies)
            {
                enemy.Move();
                enemy.Shoot(enemyBullets);
            }
            foreach(var bullet in enemyBullets)
            {
                bullet.Move();
            }
            CheckDamage();
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

        private void MovePlayer(RenderWindow window)
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