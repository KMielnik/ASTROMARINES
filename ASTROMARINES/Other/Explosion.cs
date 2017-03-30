using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;

namespace ASTROMARINES.Other
{
    public class Explosion :IDisposable
    {
        int ActualFrame;
        List<Sprite> ExplosionFrames;
        public bool ShouldBeDeleted { get => ActualFrame >= (ExplosionFrames.Count - 1); }

        public Explosion(Vector2f position, List<Sprite> explosionFrames)
        {
            ActualFrame = 0;
            ExplosionFrames = new List<Sprite>();
            foreach (var explosionFrame in explosionFrames)
                ExplosionFrames.Add(new Sprite(explosionFrame));

            foreach (var explosionFrame in ExplosionFrames)
                explosionFrame.Position = position;
        }

        public void SetExplosionScale(float scale)
        {
            foreach (var explosionFrame in ExplosionFrames)
                explosionFrame.Scale *= scale;
        }

        public void Draw(RenderWindow window)
        {
            if (ActualFrame >= ExplosionFrames.Count - 1)
                window.Draw(ExplosionFrames[ExplosionFrames.Count - 1]);
            else
            window.Draw(ExplosionFrames[ActualFrame]);
            ActualFrame++;
        }

        public void Dispose()
        {
            
        }
    }
}
