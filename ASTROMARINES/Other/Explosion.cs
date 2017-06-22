using System;
using System.Collections.Generic;
using SFML.Graphics;
using SFML.System;

namespace ASTROMARINES.Other
{
    public class Explosion :IDisposable
    {
        private int _actualFrame;
        private readonly List<Sprite> _explosionFrames;
        public bool ShouldBeDeleted => _actualFrame >= (_explosionFrames.Count - 1);

        public Explosion(Vector2f position, List<Sprite> explosionFrames)
        {
            _actualFrame = 0;
            _explosionFrames = new List<Sprite>();
            foreach (var explosionFrame in explosionFrames)
                _explosionFrames.Add(new Sprite(explosionFrame));

            foreach (var explosionFrame in _explosionFrames)
                explosionFrame.Position = position;
        }

        public void SetExplosionScale(float scale)
        {
            foreach (var explosionFrame in _explosionFrames)
                explosionFrame.Scale *= scale;
        }

        public void Draw(RenderWindow window)
        {
            if (_actualFrame >= _explosionFrames.Count - 1)
                window.Draw(_explosionFrames[_explosionFrames.Count - 1]);
            else
            window.Draw(_explosionFrames[_actualFrame]);
            _actualFrame++;
        }

        public void Dispose()
        {
            
        }
    }
}
