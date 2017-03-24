using SFML.Graphics;
using SFML.System;
using System.Collections.Generic;
using ASTROMARINES.Other;

namespace ASTROMARINES.Characters.Enemies
{
    public interface IEnemy
    {
        void Shoot(List<Bullet> EnemiesBullets);
        void Move();
        void DrawEnemy(RenderWindow window);
        void Damaged();
        bool ShouldBeDeleted { get; }
        FloatRect BoudingBox { get; }
    }
}
