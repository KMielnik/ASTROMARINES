using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASTROMARINES
{
    public interface IPlayer
    {
        void Move();
        List<Bullet> GetBullets();
        void Shoot();
        void DrawPlayer(RenderWindow window);
        void Damaged();
        void LevelUp();
        bool ShouldBeDeleted { get; }
        FloatRect BoundingBox { get; }
    }
}
