using SFML.Graphics;
using System;
using System.Collections.Generic;

namespace ASTROMARINES
{
    interface IEnemyFactory
    {
        IEnemy CreateEnemy(EnemyTypes enemyType);
    }

    class EnemyFactory : IEnemyFactory
    {
        private List<Texture> enemyTextures;

        public EnemyFactory(List<Texture> enemyTextures)
        {
            this.enemyTextures = enemyTextures;
        }

        public IEnemy CreateEnemy(EnemyTypes enemyType)
        {
            switch(enemyType)
            {
                case EnemyTypes.PowerUp:
                    return new Enemy1(enemyTextures);
                case EnemyTypes.Enemy2:
                    return new Enemy2(enemyTextures);
                case EnemyTypes.Enemy3:
                    return new Enemy3(enemyTextures);
                case EnemyTypes.Enemy4:
                    return new Enemy4(enemyTextures);
                default:
                    throw new Exception("You tried to create non-existing enemy");
            }
        }
    }
}
