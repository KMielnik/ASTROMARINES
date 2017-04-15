using System;
using SFML.Graphics;

namespace ASTROMARINES.Levels
{
    internal interface ILevel: IDisposable
    {
        void LevelLogic(RenderWindow window);
        void Draw(RenderWindow window);
        bool HasLevelEnded { get; }
    }
}