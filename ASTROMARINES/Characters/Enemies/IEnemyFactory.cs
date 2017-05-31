using System;
using System.Collections.Generic;
using ASTROMARINES.Properties;
using SFML.Graphics;
using SFML.System;

namespace ASTROMARINES.Characters.Enemies
{
    public interface IEnemyFactory: IDisposable
    {
        IEnemy CreateEnemy(EnemyTypes enemyType);
        IEnemy CreateRandomEnemy();
        bool IsNewEnemyAvalible();
        bool IsPowerUpAvalible();
    }

    public class EnemyFactory : IEnemyFactory
    {
        private List<Texture> enemyTextures;
        private Clock enemyReloadClock;
        private Clock powerupReloadClock;

        public EnemyFactory()
        {
            enemyTextures = new List<Texture>();
            enemyTextures.Add(new Texture(Resources.Enemy1));
            enemyTextures.Add(new Texture(Resources.Enemy2));
            enemyTextures.Add(new Texture(Resources.Enemy3));
            enemyTextures.Add(new Texture(Resources.Enemy4));

            enemyTextures.Add(new Texture(Resources.BossMainCannon));
            enemyTextures.Add(new Texture(Resources.Boss));

            enemyReloadClock = new Clock();
            powerupReloadClock = new Clock();
        }

        public IEnemy CreateEnemy(EnemyTypes enemyType)
        {
            switch (enemyType)
            {
                case EnemyTypes.PowerUp:
                    powerupReloadClock.Restart();
                    return new Enemy1(enemyTextures[(int)EnemyTypes.PowerUp]);
                case EnemyTypes.Enemy2:
                    enemyReloadClock.Restart();
                    return new Enemy2(enemyTextures[(int)EnemyTypes.Enemy2]);
                case EnemyTypes.Enemy3:
                    enemyReloadClock.Restart();
                    return new Enemy3(enemyTextures[(int)EnemyTypes.Enemy3]);
                case EnemyTypes.Enemy4:
                    enemyReloadClock.Restart();
                    return new Enemy4(enemyTextures[(int)EnemyTypes.Enemy4]);
                case EnemyTypes.Boss:
                    return new Boss(enemyTextures[(int)EnemyTypes.Boss],
                                    enemyTextures[(int)EnemyTypes.Boss + 1]);
                default:
                    throw new Exception("You tried to create non-existing enemy");
            }
        }

        public IEnemy CreateRandomEnemy()
        {
            var random = new Random();
            var enemyValues = Enum.GetValues(typeof(EnemyTypes));
            var randomEnemy = (EnemyTypes)enemyValues.GetValue(random.Next(1, (int)EnemyTypes.Boss));

            return CreateEnemy(randomEnemy);
        }

        public void Dispose()
        {
            foreach (var enemyTexture in enemyTextures)
                enemyTexture.Dispose();
            enemyReloadClock.Dispose();
            powerupReloadClock.Dispose();
        }

        public bool IsNewEnemyAvalible()
        {
            return enemyReloadClock.ElapsedTime.AsSeconds() > 1.5;
        }

        public bool IsPowerUpAvalible()
        {
            return powerupReloadClock.ElapsedTime.AsSeconds() > 15;
        }
    }
}
