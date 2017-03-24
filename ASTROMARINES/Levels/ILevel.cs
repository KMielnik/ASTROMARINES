using SFML.Graphics;

namespace ASTROMARINES.Levels
{
    interface ILevel
    {
        void LevelLogic(RenderWindow window);
        void Draw(RenderWindow window);
        bool HasLevelEnded { get; }
    }
}