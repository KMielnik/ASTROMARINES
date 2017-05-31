using ASTROMARINES.Levels;
using SFML.Graphics;
using SFML.Window;

namespace ASTROMARINES
{
    public class Program
    {
        public static RenderWindow window;

        private static void Main()
        {
            window = new RenderWindow(new VideoMode(1500, 880), "dsada");
            window.KeyPressed += Window_KeyPressed;
            window.Closed += (s, a) => window.Close();

            var game = new Game(window);
            
            while(window.IsOpen)
            {
                window.DispatchEvents();
                game.Run();
            }

            game.Dispose();
        }

        private static void Window_KeyPressed(object sender, KeyEventArgs e)
        {
            if(e.Code == Keyboard.Key.Escape)
            {
                window.Close();
            }
        }
    }


}