using ASTROMARINES.Other;
using ASTROMARINES.Properties;
using SFML.Graphics;
using SFML.System;
using System.Collections.Generic;

namespace ASTROMARINES.Characters.Player
{
    partial class Player
    {
        int hp;
        int hpMax;
        Texture playerTexture;
        Sprite playerSprite;
        Vector2f dimensions;
        Vector2f speedVector;
        Weapon weapon;
        PlayerLevel playerLevel;
        HpBar hpBar;
        public List<Bullet> Bullets { get; private set; }
        public Vector2f Position { get => playerSprite.Position; }

        public Player()
        {
            playerTexture = new Texture(Resources.Player);

            playerSprite = new Sprite(playerTexture);
            playerSprite.Scale = new Vector2f(0.25f * WindowProperties.ScaleX,
                                              0.25f * WindowProperties.ScaleY);
            playerSprite.Origin = new Vector2f(128, 128);
            dimensions.X = 256 * 0.25f * WindowProperties.ScaleX;
            dimensions.Y = 256 * 0.25f * WindowProperties.ScaleY;
            playerSprite.Position = new Vector2f(WindowProperties.WindowWidth / 2,
                                                 WindowProperties.WindowHeight - dimensions.Y);
            playerLevel = new PlayerLevel();
            playerLevel = PlayerLevel.Level3;

            weapon = new Weapon();
            hpBar = new HpBar(dimensions);
            Bullets = new List<Bullet>();

            hpMax = 250;
            hp = hpMax;
        }


        public bool ShouldBeDeleted { get => hp <= 0; }

        public FloatRect BoundingBox { get => playerSprite.GetGlobalBounds(); }

        public void Damaged()
        {
            hp--;
            hpBar.UpdateHpBarSize(hp, hpMax);
        }

        public void Draw(RenderWindow window)
        {
            window.Draw(playerSprite);
            weapon.SetWeaponPosition(Position, dimensions, window);
            weapon.Draw(window, playerLevel);
            foreach (var bullet in Bullets)
                bullet.Draw(window);
            hpBar.Draw(window);
        }

        public void LevelUp()
        {
            switch (playerLevel)
            {
                case PlayerLevel.Level1:
                    playerLevel = PlayerLevel.Level2;
                    break;
                case PlayerLevel.Level2:
                    playerLevel = PlayerLevel.Level3;
                    break;
                case PlayerLevel.Level3:
                    playerLevel = PlayerLevel.Level4;
                    break;
                case PlayerLevel.Level4:
                    break;
            }
        }

        public void Move()
        {
            foreach (var bullet in Bullets)
                bullet.Move();

            BounceIfPleyerTriesToEscapeMap();
            playerSprite.Position += speedVector;
            speedVector /= 1.08f;
            hpBar.SetHpBarPositon(Position, dimensions);
            DeleteOldBullets();
        }

        private void DeleteOldBullets()
        {
            for(int i=0;i<Bullets.Count;i++)
                if(Bullets[i].ShouldBeDeleted)
                {
                    Bullets[i].Dispose();
                    Bullets[i] = null;
                    Bullets.RemoveAt(i);
                }
        }

        void BounceIfPleyerTriesToEscapeMap()
        {
            bool flewOutOfLeftSide = Position.X < dimensions.X / 1.5;
            bool flewOutOfRightSide = Position.X > (WindowProperties.WindowWidth - dimensions.X / 1.5);
            bool flewOutOfTheTop = Position.Y < dimensions.Y / 1.5;
            bool flewOutOfTheBottom = Position.Y > (WindowProperties.WindowHeight - dimensions.Y / 1.3);

            if (flewOutOfLeftSide || flewOutOfRightSide)
            {
                speedVector.X = -speedVector.X * 1.3f;
                speedVector.Y = speedVector.Y * 1.3f;
            }

            if (flewOutOfTheBottom || flewOutOfTheTop)
            {
                speedVector.X = speedVector.X * 1.3f;
                speedVector.Y = -speedVector.Y * 1.3f;
            }
        }

        public void Shoot(RenderWindow window)
        {
            var newBullets = weapon.Shoot(playerLevel, Position, dimensions, window);
            Bullets.AddRange(newBullets);
        }

        public void AccelerateUp()
        {
            speedVector += new Vector2f(0, -0.7f);
        }

        public void AccelerateDown()
        {
            speedVector += new Vector2f(0, 0.7f);
        }

        public void AccelerateLeft()
        {
            speedVector += new Vector2f(-0.7f, 0);
        }

        public void AccelerateRight()
        {
            speedVector += new Vector2f(0.7f, 0);
        }

        public void Dispose()
        {
            playerTexture.Dispose();
            playerSprite.Dispose();
            weapon.Dispose();
            hpBar.Dispose();
            foreach (var bullet in Bullets)
                bullet.Dispose();
        }
    }
}
