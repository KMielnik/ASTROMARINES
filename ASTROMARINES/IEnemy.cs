using SFML.Graphics;
using SFML.System;
using System.Collections.Generic;

namespace ASTROMARINES
{
    public interface IEnemy
    {
        void Shoot(List<Bullet> EnemiesBullets);
        void Move();
        void DrawEnemy(RenderWindow window);
        void Damaged();
        bool ShouldBeDeleted { get; }
        Vector2f Position { get; }
        FloatRect BoudingBox { get; }
    }
}
