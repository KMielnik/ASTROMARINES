using System;
using System.Collections.Generic;
using SFML.Graphics;
using SFML.System;

namespace ASTROMARINES.Other
{
    public class Explosion :IDisposable
    {
        private int actualFrame;
        private readonly List<Sprite> explosionFrames;
        public bool ShouldBeDeleted => actualFrame >= (explosionFrames.Count - 1);

        public Explosion(Vector2f position, IEnumerable<Sprite> explosionFrames)
        {
            actualFrame = 0;
            this.explosionFrames = new List<Sprite>();
            foreach (var explosionFrame in explosionFrames)
                this.explosionFrames.Add(new Sprite(explosionFrame));

            foreach (var explosionFrame in this.explosionFrames)
                explosionFrame.Position = position;
        }

        public void SetExplosionScale(float scale)
        {
            foreach (var explosionFrame in explosionFrames)
                explosionFrame.Scale *= scale;
        }

        public void Draw(RenderWindow window)
        {
            window.Draw(actualFrame >= explosionFrames.Count - 1
                ? explosionFrames[explosionFrames.Count - 1]
                : explosionFrames[actualFrame]);
            actualFrame++;
        }

        public void Dispose()
        {
            
        }
    }
}
