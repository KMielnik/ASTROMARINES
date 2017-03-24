using SFML.Graphics;
using SFML.System;

namespace ASTROMARINES.Characters
{
    class HPBar
    {
        RectangleShape HPBarMeter;
        RectangleShape HPBarBackground;

        public HPBar(Vector2f characterDimensions)
        {
            HPBarMeter = new RectangleShape();
            HPBarBackground = new RectangleShape();

            HPBarMeter.Size = new Vector2f(characterDimensions.X, 3);
            HPBarMeter.Origin = new Vector2f(characterDimensions.X / 2, 1.5f);
            HPBarMeter.FillColor = new Color(Color.Red);
            HPBarBackground.Size = new Vector2f(characterDimensions.X + 2, 5);
            HPBarBackground.Origin = new Vector2f(characterDimensions.X / 2 + 1, 2.5f);
            HPBarBackground.FillColor = new Color(Color.White);
        }

        public void SetHPBarPositon(Vector2f characterPosition, Vector2f characterDimensions)
        {
            var newHPBarPosition = new Vector2f(characterPosition.X,
                                                characterPosition.Y + (characterDimensions.Y * 7 / 10));
            HPBarMeter.Position = newHPBarPosition;
            HPBarBackground.Position = newHPBarPosition;
        }

        public void Draw(RenderWindow window)
        {
            window.Draw(HPBarBackground);
            window.Draw(HPBarMeter);
        }

        public void UpdateHPBarSize(float HP, float HPMax)
        {
            var newHPBarSize = new Vector2f((HPBarBackground.Size.X - 2) * HP / HPMax, 3);
            HPBarMeter.Size = newHPBarSize;
        }
    }
}
