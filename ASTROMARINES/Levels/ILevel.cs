using SFML.Graphics;
using System;

namespace ASTROMARINES.Levels
{
    interface ILevel: IDisposable
    {
        void LevelLogic(RenderWindow window);
        void Draw(RenderWindow window);
        bool HasLevelEnded { get; }
    }
}