using ASTROMARINES.Other;
using ASTROMARINES.Properties;
using SFML.Graphics;
using SFML.System;
using System.Collections.Generic;

namespace ASTROMARINES.Characters.Player
{
    partial class Player
    {
        int _hp;
        readonly int _hpMax;
        readonly Texture _playerTexture;
        readonly Sprite _playerSprite;
        Vector2f _dimensions;
        Vector2f _speedVector;
        readonly Weapon _weapon;
        PlayerLevel _playerLevel;
        readonly HpBar _hpBar;
        public List<Bullet> Bullets { get; private set; }
        public Vector2f Position { get => _playerSprite.Position; }

        public Player()
        {
            _playerTexture = new Texture(Resources.Player);

            _playerSprite = new Sprite(_playerTexture);
            _playerSprite.Scale = new Vector2f(0.25f * WindowProperties.ScaleX,
                                              0.25f * WindowProperties.ScaleY);
            _playerSprite.Origin = new Vector2f(128, 128);
            _dimensions.X = 256 * 0.25f * WindowProperties.ScaleX;
            _dimensions.Y = 256 * 0.25f * WindowProperties.ScaleY;
            _playerSprite.Position = new Vector2f(WindowProperties.WindowWidth / 2,
                                                 WindowProperties.WindowHeight - _dimensions.Y);
            _playerLevel = new PlayerLevel();
            _playerLevel = PlayerLevel.Level1;

            _weapon = new Weapon();
            _hpBar = new HpBar(_dimensions);
            Bullets = new List<Bullet>();

            _hpMax = 250;
            _hp = _hpMax;
        }


        public bool ShouldBeDeleted { get => _hp <= 0; }

        public FloatRect BoundingBox { get => _playerSprite.GetGlobalBounds(); }

        public void Damaged()
        {
            _hp--;
            _hpBar.UpdateHpBarSize(_hp, _hpMax);
        }

        public void Draw(RenderWindow window)
        {
            window.Draw(_playerSprite);
            _weapon.SetWeaponPosition(Position, _dimensions, window);
            _weapon.Draw(window, _playerLevel);
            foreach (var bullet in Bullets)
                bullet.Draw(window);
            _hpBar.Draw(window);
        }

        public void LevelUp()
        {
            switch (_playerLevel)
            {
                case PlayerLevel.Level1:
                    _playerLevel = PlayerLevel.Level2;
                    break;
                case PlayerLevel.Level2:
                    _playerLevel = PlayerLevel.Level3;
                    break;
                case PlayerLevel.Level3:
                    _playerLevel = PlayerLevel.Level4;
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
            _playerSprite.Position += _speedVector;
            _speedVector /= 1.08f;
            _hpBar.SetHpBarPositon(Position, _dimensions);
            DeleteOldBullets();
        }

        private void DeleteOldBullets()
        {
            for(var i=0;i<Bullets.Count;i++)
                if(Bullets[i].ShouldBeDeleted)
                {
                    Bullets[i].Dispose();
                    Bullets[i] = null;
                    Bullets.RemoveAt(i);
                }
        }

        void BounceIfPleyerTriesToEscapeMap()
        {
            var flewOutOfLeftSide = Position.X < _dimensions.X / 1.5;
            var flewOutOfRightSide = Position.X > (WindowProperties.WindowWidth - _dimensions.X / 1.5);
            var flewOutOfTheTop = Position.Y < _dimensions.Y / 1.5;
            var flewOutOfTheBottom = Position.Y > (WindowProperties.WindowHeight - _dimensions.Y / 1.3);

            if (flewOutOfLeftSide || flewOutOfRightSide)
            {
                _speedVector.X = -_speedVector.X * 1.3f;
                _speedVector.Y = _speedVector.Y * 1.3f;
            }

            if (flewOutOfTheBottom || flewOutOfTheTop)
            {
                _speedVector.X = _speedVector.X * 1.3f;
                _speedVector.Y = -_speedVector.Y * 1.3f;
            }
        }

        public void Shoot(RenderWindow window)
        {
            var newBullets = _weapon.Shoot(_playerLevel, Position, _dimensions, window);
            Bullets.AddRange(newBullets);
        }

        public void AccelerateUp()
        {
            _speedVector += new Vector2f(0, -0.7f);
        }

        public void AccelerateDown()
        {
            _speedVector += new Vector2f(0, 0.7f);
        }

        public void AccelerateLeft()
        {
            _speedVector += new Vector2f(-0.7f, 0);
        }

        public void AccelerateRight()
        {
            _speedVector += new Vector2f(0.7f, 0);
        }

        public void Dispose()
        {
            _playerTexture.Dispose();
            _playerSprite.Dispose();
            _weapon.Dispose();
            _hpBar.Dispose();
            foreach (var bullet in Bullets)
                bullet.Dispose();
        }
    }
}
