using System;
using System.Collections.Generic;
using SFML.Graphics;
using SFML.System;

namespace ASTROMARINES
{
    public partial class Player : IPlayer
    {
        int HP;
        int HPMax;
        Texture playerTexture;
        Sprite playerSprite;
        Vector2f dimensions;
        Vector2f speedVector;
        MousePointer mousePointer;
        Weapon weapon;


        public bool ShouldBeDeleted { get => HP <= 0; }

        public FloatRect BoundingBox => throw new NotImplementedException();

        public void Damaged()
        {
            throw new NotImplementedException();
        }

        public void DrawPlayer(RenderWindow window)
        {
            throw new NotImplementedException();
        }

        public List<Bullet> GetBullets()
        {
            throw new NotImplementedException();
        }

        public void LevelUp()
        {
            throw new NotImplementedException();
        }

        public void Move()
        {
            throw new NotImplementedException();
        }

        public void Shoot()
        {
            throw new NotImplementedException();
        }
    }
}
