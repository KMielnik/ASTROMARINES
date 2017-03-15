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
                    throw new Exception("This enemy hasn't been implemented yet :D");
                case EnemyTypes.Enemy3:
                    throw new Exception("This enemy hasn't been implemented yet :D");
                case EnemyTypes.Enemy4:
                    throw new Exception("This enemy hasn't been implemented yet :D");
                default:
                    throw new Exception("You tried to create non-existing enemy");
            }
        }
    }
}
