using SFML.Graphics;
using SFML.System;
using System.Collections.Generic;
using ASTROMARINES.Other;
using System;

namespace ASTROMARINES.Characters.Enemies
{
    public interface IEnemy : IDisposable
    {
        void Shoot(List<Bullet> EnemiesBullets);
        void Move();
        void Draw(RenderWindow window);
        void Damaged();
        bool ShouldBeDeleted { get; set; }
        Vector2f Position { get; }
        FloatRect BoudingBox { get; }
    }
}
