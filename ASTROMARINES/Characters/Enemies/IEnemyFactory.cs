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
        private readonly List<Texture> _enemyTextures;
        private readonly Clock _enemyReloadClock;
        private readonly Clock _powerupReloadClock;

        public EnemyFactory()
        {
            _enemyTextures = new List<Texture>();
            _enemyTextures.Add(new Texture(Resources.Enemy1));
            _enemyTextures.Add(new Texture(Resources.Enemy2));
            _enemyTextures.Add(new Texture(Resources.Enemy3));
            _enemyTextures.Add(new Texture(Resources.Enemy4));

            _enemyTextures.Add(new Texture(Resources.BossMainCannon));
            _enemyTextures.Add(new Texture(Resources.Boss));

            _enemyReloadClock = new Clock();
            _powerupReloadClock = new Clock();
        }

        public IEnemy CreateEnemy(EnemyTypes enemyType)
        {
            switch (enemyType)
            {
                case EnemyTypes.PowerUp:
                    _powerupReloadClock.Restart();
                    return new Enemy1(_enemyTextures[(int)EnemyTypes.PowerUp]);
                case EnemyTypes.Enemy2:
                    _enemyReloadClock.Restart();
                    return new Enemy2(_enemyTextures[(int)EnemyTypes.Enemy2]);
                case EnemyTypes.Enemy3:
                    _enemyReloadClock.Restart();
                    return new Enemy3(_enemyTextures[(int)EnemyTypes.Enemy3]);
                case EnemyTypes.Enemy4:
                    _enemyReloadClock.Restart();
                    return new Enemy4(_enemyTextures[(int)EnemyTypes.Enemy4]);
                case EnemyTypes.Boss:
                    return new Boss(_enemyTextures[(int)EnemyTypes.Boss],
                                    _enemyTextures[(int)EnemyTypes.Boss + 1]);
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
            foreach (var enemyTexture in _enemyTextures)
                enemyTexture.Dispose();
            _enemyReloadClock.Dispose();
            _powerupReloadClock.Dispose();
        }

        public bool IsNewEnemyAvalible()
        {
            return _enemyReloadClock.ElapsedTime.AsSeconds() > 1.5;
        }

        public bool IsPowerUpAvalible()
        {
            return _powerupReloadClock.ElapsedTime.AsSeconds() > 15;
        }
    }
}
