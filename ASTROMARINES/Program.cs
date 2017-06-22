using ASTROMARINES.Levels;

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