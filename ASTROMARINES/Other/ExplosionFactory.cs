using System;
using System.Collections.Generic;
using ASTROMARINES.Properties;
using SFML.Graphics;
using SFML.System;

namespace ASTROMARINES.Other
{
    public class ExplosionFactory : IDisposable
    {
        private readonly List<Sprite> explosionFrames = new List<Sprite>();
        private readonly Texture explosionTexture;
        public ExplosionFactory()
        {
            explosionTexture = new Texture(Resources.ExplosionSprite);

            for(var i=0;i<9;i++)
                for(var j=0;j<9;j++)
                {
                    var explosionFrame = new Sprite(explosionTexture)
                    {
                        Origin = new Vector2f(50, 40),
                        Scale = new Vector2f(WindowProperties.ScaleX, WindowProperties.ScaleY),
                        TextureRect = new IntRect(j * 100, i * 100, 100, 100)
                    };
                    explosionFrames.Add(explosionFrame);
                }
        }

        public Explosion CreateExplosion(Vector2f position)
        {
            return new Explosion(position,explosionFrames);
        }

        public void Dispose()
        {
            foreach (var explosionFrame in explosionFrames)
                explosionFrame.Dispose();
            explosionTexture.Dispose();
        }
    }
}
