using ASTROMARINES.Levels;
using SFML.Graphics;
using SFML.Window;

namespace ASTROMARINES
{
    public static class Program
    {
        private static void Main()
        {
            var game = new Game();
            game.Run();
            game.Dispose();
        }
    }
}