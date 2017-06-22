using System;
using SFML.Graphics;
using SFML.System;

namespace ASTROMARINES.Characters
{
    internal class HpBar : IDisposable
    {
        private readonly RectangleShape _hpBarMeter;
        private readonly RectangleShape _hpBarBackground;

        public HpBar(Vector2f characterDimensions)
        {
            _hpBarMeter = new RectangleShape();
            _hpBarBackground = new RectangleShape();

            _hpBarMeter.Size = new Vector2f(characterDimensions.X, 3);
            _hpBarMeter.Origin = new Vector2f(characterDimensions.X / 2, 1.5f);
            _hpBarMeter.FillColor = new Color(Color.Red);
            _hpBarBackground.Size = new Vector2f(characterDimensions.X + 2, 5);
            _hpBarBackground.Origin = new Vector2f(characterDimensions.X / 2 + 1, 2.5f);
            _hpBarBackground.FillColor = new Color(Color.White);
        }

        public void SetHpBarPositon(Vector2f characterPosition, Vector2f characterDimensions)
        {
            var newHpBarPosition = new Vector2f(characterPosition.X,
                                                characterPosition.Y + (characterDimensions.Y * 7 / 10));
            _hpBarMeter.Position = newHpBarPosition;
            _hpBarBackground.Position = newHpBarPosition;
        }

        public void Draw(RenderWindow window)
        {
            window.Draw(_hpBarBackground);
            window.Draw(_hpBarMeter);
        }

        public void UpdateHpBarSize(float hp, float hpMax)
        {
            var newHpBarSize = new Vector2f((_hpBarBackground.Size.X - 2) * hp / hpMax, 3);
            _hpBarMeter.Size = newHpBarSize;
        }

        public void Dispose()
        {
            _hpBarMeter.Dispose();
            _hpBarBackground.Dispose();
        }
    }
}
