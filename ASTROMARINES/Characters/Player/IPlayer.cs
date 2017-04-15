using System;
using System.Collections.Generic;
using ASTROMARINES.Other;
using SFML.Graphics;

namespace ASTROMARINES.Characters.Player
{
    internal interface IPlayer : IDisposable
    {
        void Move();
        void AccelerateUp();
        void AccelerateDown();
        void AccelerateLeft();
        void AccelerateRight();
        void Shoot(RenderWindow window);
        void Draw(RenderWindow window);
        void Damaged();
        void LevelUp();
        bool ShouldBeDeleted { get; }
        FloatRect BoundingBox { get; }
        List<Bullet> Bullets { get; }
    }
}
