using ASTROMARINES.Properties;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;

namespace ASTROMARINES.Other
{
    public class ExplosionFactory
    {
        List<Sprite> ExplosionFrames = new List<Sprite>();
        Texture ExplosionTexture;
        public ExplosionFactory()
        {
            ExplosionTexture = new Texture(Resources.ExplosionSprite);

            for(int i=0;i<9;i++)
                for(int j=0;j<9;j++)
                {
                   Sprite explosionFrame = new Sprite(ExplosionTexture);
                    explosionFrame.Origin = new Vector2f(50, 40);
                    explosionFrame.Scale = new Vector2f(WindowProperties.ScaleX, WindowProperties.ScaleY);
                    explosionFrame.TextureRect = new IntRect(j * 100, i * 100, 100, 100);
                    ExplosionFrames.Add(explosionFrame);
                }
        }

        public Explosion CreateExplosion(Vector2f position)
        {
            return new Explosion(position,ExplosionFrames);
        }
    }
}
