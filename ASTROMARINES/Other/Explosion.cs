using SFML.Graphics;
using SFML.System;
using System.Collections.Generic;

namespace ASTROMARINES.Other
{
    public class Explosion
    {
        int ActualFrame;
        List<Sprite> ExplosionFrames;
        public bool ShouldBeDeleted { get => ActualFrame > (ExplosionFrames.Count - 1); }

        public Explosion(Vector2f position, List<Sprite> explosionFrames)
        {
            ExplosionFrames = new List<Sprite>(explosionFrames);
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
            window.Draw(ExplosionFrames[ActualFrame]);
            ActualFrame++;
        }
    }
}
