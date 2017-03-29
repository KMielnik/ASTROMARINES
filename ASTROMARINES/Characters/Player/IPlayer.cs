using ASTROMARINES.Other;
using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASTROMARINES.Characters.Player
{
    interface IPlayer : IDisposable
    {
        void Move();
        void AccelerateUp();
        void AccelerateDown();
        void AccelerateLeft();
        void AccelerateRight();
        void Shoot(RenderWindow window);
        void DrawPlayer(RenderWindow window);
        void Damaged();
        void LevelUp();
        bool ShouldBeDeleted { get; }
        FloatRect BoundingBox { get; }
        List<Bullet> Bullets { get; }
    }
}
