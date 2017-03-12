using System.Collections.Generic;

namespace ASTROMARINES
{
    public interface IEnemy
    {
        void Shoot(List<Bullet> EnemiesBullets);
    }
}
