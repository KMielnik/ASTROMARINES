using System;
using SFML.Graphics;
using SFML.System;

namespace ASTROMARINES.Characters
{
    internal class HpBar : IDisposable
    {
        private RectangleShape hpBarMeter;
        private RectangleShape hpBarBackground;

        public HpBar(Vector2f characterDimensions)
        {
            hpBarMeter = new RectangleShape();
            hpBarBackground = new RectangleShape();

            hpBarMeter.Size = new Vector2f(characterDimensions.X, 3);
            hpBarMeter.Origin = new Vector2f(characterDimensions.X / 2, 1.5f);
            hpBarMeter.FillColor = new Color(Color.Red);
            hpBarBackground.Size = new Vector2f(characterDimensions.X + 2, 5);
            hpBarBackground.Origin = new Vector2f(characterDimensions.X / 2 + 1, 2.5f);
            hpBarBackground.FillColor = new Color(Color.White);
        }

        public void SetHpBarPositon(Vector2f characterPosition, Vector2f characterDimensions)
        {
            var newHpBarPosition = new Vector2f(characterPosition.X,
                                                characterPosition.Y + (characterDimensions.Y * 7 / 10));
            hpBarMeter.Position = newHpBarPosition;
            hpBarBackground.Position = newHpBarPosition;
        }

        public void Draw(RenderWindow window)
        {
            window.Draw(hpBarBackground);
            window.Draw(hpBarMeter);
        }

        public void UpdateHpBarSize(float hp, float hpMax)
        {
            var newHpBarSize = new Vector2f((hpBarBackground.Size.X - 2) * hp / hpMax, 3);
            hpBarMeter.Size = newHpBarSize;
        }

        public void Dispose()
        {
            hpBarMeter.Dispose();
            hpBarBackground.Dispose();
        }
    }
}
