using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;

namespace ASTROMARINES.Characters.Enemies
{
    public interface IEnemyFactory
    {
        IEnemy CreateEnemy(EnemyTypes enemyType);
        bool IsNewEnemyAvalible();
        bool IsPowerUpAvalible();
    }

    public class EnemyFactory : IEnemyFactory
    {
        private List<Texture> enemyTextures;
        private Clock enemyReloadClock;
        private Clock powerupReloadClock;

        public EnemyFactory(List<Texture> enemyTextures)
        {
            this.enemyTextures = enemyTextures;
            enemyReloadClock = new Clock();
            powerupReloadClock = new Clock();
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

        public IEnemy CreateRandomEnemy()
        {
            var random = new Random();
            var EnemyValues = Enum.GetValues(typeof(EnemyTypes));
            var randomEnemy = (EnemyTypes)EnemyValues.GetValue(random.Next(EnemyValues.Length));

            return CreateEnemy(randomEnemy);
        }

        public bool IsNewEnemyAvalible()
        {
            return enemyReloadClock.ElapsedTime.AsSeconds() > 3;
        }

        public bool IsPowerUpAvalible()
        {
            return powerupReloadClock.ElapsedTime.AsSeconds() > 15;
        }
    }
}
