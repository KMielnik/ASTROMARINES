using SFML.Graphics;

namespace ASTROMARINES
{
    public interface ILevel
    {
        void LevelLogic(RenderWindow window);
        void Draw(RenderWindow window);
        bool HasLevelEnded { get; }
    }
}