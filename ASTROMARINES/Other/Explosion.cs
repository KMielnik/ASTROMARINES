using System;
using System.Collections.Generic;
using SFML.Graphics;
using SFML.System;

namespace ASTROMARINES.Other
{
    public class Explosion :IDisposable
    {
        private int actualFrame;
        private List<Sprite> explosionFrames;
        public bool ShouldBeDeleted => actualFrame >= (explosionFrames.Count - 1);

        public Explosion(Vector2f position, List<Sprite> explosionFrames)
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
            if (actualFrame >= explosionFrames.Count - 1)
                window.Draw(explosionFrames[explosionFrames.Count - 1]);
            else
            window.Draw(explosionFrames[actualFrame]);
            actualFrame++;
        }

        public void Dispose()
        {
            
        }
    }
}
