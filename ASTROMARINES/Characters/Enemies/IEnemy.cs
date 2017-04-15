using System;
using System.Collections.Generic;
using ASTROMARINES.Other;
using SFML.Graphics;
using SFML.System;

namespace ASTROMARINES.Characters.Enemies
{
    public interface IEnemy : IDisposable
    {
        void Shoot(List<Bullet> enemiesBullets);
        void Move();
        void Draw(RenderWindow window);
        void Damaged();
        bool ShouldBeDeleted { get; set; }
        Vector2f Position { get; }
        FloatRect BoudingBox { get; }
    }
}
