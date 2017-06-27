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
    // ReSharper disable once UnusedMember.Global
    internal class LevelBoss : ILevel
    {
        private readonly Texture backgroundTexture;
        private readonly Sprite background;
        private readonly IPlayer player;
        private readonly IEnemyFactory enemyFactory;
        private readonly IEnemy boss;
        private readonly List<Bullet> bossBullets;
        private readonly ExplosionFactory explosionFactory;
        private readonly List<Explosion> explosions;
        private readonly Clock levelClock;
        private readonly Music backgroundMusic;
        private readonly MousePointer mousePointer;
        private readonly Music bossDeathSound;

        private bool hasBossDied;
        private bool hasLevelEnded;

        public bool HasLevelEnded
        {
            get => (hasBossDied && levelClock.ElapsedTime.AsSeconds() > 5) || hasLevelEnded;
            private set => hasLevelEnded = value;
        }

        public LevelBoss(IPlayer player)
        {
            backgroundTexture = new Texture(Resources.LevelBG);
            background = new Sprite(backgroundTexture)
            {
                Scale = new Vector2f(WindowProperties.ScaleX, WindowProperties.ScaleY)
            };

            this.player = player;

            mousePointer = new MousePointer();

            enemyFactory = new EnemyFactory();
            boss = enemyFactory.CreateEnemy(EnemyTypes.Boss);
            bossBullets = new List<Bullet>();
            explosionFactory = new ExplosionFactory();
            explosions = new List<Explosion>();
            levelClock = new Clock();

            backgroundMusic = new Music(Resources.BossBGMusic) {Loop = true};
            backgroundMusic.Play();

            bossDeathSound = new Music(Resources.ExplosionSound);
        }

        public void Dispose()
        {
            HasLevelEnded = true;

            backgroundTexture.Dispose();
            background.Dispose();
            enemyFactory.Dispose();
            boss.Dispose();
            explosionFactory.Dispose();
            levelClock.Dispose();
            mousePointer.Dispose();
            backgroundMusic.Stop();
            backgroundMusic.Dispose();
        }

        public void Draw(RenderWindow window)
        {
            window.Clear(Color.Black);

            window.Draw(background);
            if (hasBossDied == false)
                boss.Draw(window);
            foreach (var bullet in bossBullets)
                bullet.Draw(window, Color.Cyan);
            player.Draw(window);
            foreach (var explosion in explosions)
                explosion.Draw(window);
            mousePointer.Draw(window);

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
            player.Move();

            if (hasBossDied == false)
            {
                boss.Move();
                boss.Shoot(bossBullets);
            }

            foreach (var bullet in bossBullets)
            {
                bullet.Move();
            }
        }

        private void DeleteObjectsSetForDeletion()
        {
            if (boss.ShouldBeDeleted)
            {
                if (hasBossDied == false)
                {
                    hasBossDied = true;
                    levelClock.Restart();
                    bossDeathSound.Play();
                }
            }
            for (var i = 0; i < bossBullets.Count; i++)
                if (bossBullets[i].ShouldBeDeleted)
                {
                    bossBullets[i].Dispose();
                    bossBullets[i] = null;
                    bossBullets.RemoveAt(i);
                }
            for (var i = 0; i < explosions.Count; i++)
                if (explosions[i].ShouldBeDeleted)
                {
                    explosions[i].Dispose();
                    explosions[i] = null;
                    explosions.RemoveAt(i);
                }
        }

        private void CreateExplosionsIfBossDied()
        {
            if (!hasBossDied) return;

            if (!(levelClock.ElapsedTime.AsSeconds() < 2)) return;

            if (levelClock.ElapsedTime.AsMilliseconds() % 5 != 0) return;

            var random = new Random();
            var randXInBoss = random.Next((int)(boss.BoudingBox.Left),
                (int)(boss.BoudingBox.Left + boss.BoudingBox.Width * 1.5f));
            var randYInBoss = random.Next((int)(boss.BoudingBox.Top),
                (int)(boss.BoudingBox.Top + boss.BoudingBox.Height));
            var explosion = explosionFactory.CreateExplosion(new Vector2f(randXInBoss, randYInBoss));
            explosion.SetExplosionScale(random.Next(1, 5));

            explosions.Add(explosion);
        }

        private void CheckDamage()
        {
            if (boss.ShouldBeDeleted == false)
            {
                if (player.BoundingBox.Intersects(boss.BoudingBox))
                    player.Damaged();

                foreach (var playerBullet in player.Bullets)
                    if (boss.BoudingBox.Contains(playerBullet.Position.X, playerBullet.Position.Y))
                    {
                        boss.Damaged();
                        playerBullet.ShouldBeDeleted = true;
                        explosions.Add(explosionFactory.CreateExplosion(playerBullet.Position));
                    }
            }

            foreach (var bossBullet in bossBullets)
            {
                if (player.BoundingBox.Contains(bossBullet.Position.X, bossBullet.Position.Y))
                {
                    player.Damaged();
                    bossBullet.ShouldBeDeleted = true;
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
    }
}