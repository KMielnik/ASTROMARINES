using ASTROMARINES.Levels;
using SFML.Graphics;
using SFML.Window;

namespace ASTROMARINES
{
    public class Program
    {
        public static RenderWindow Window;

        private static void Main()
        {
            Window = new RenderWindow(new VideoMode(1500, 880), "dsada");
            Window.KeyPressed += Window_KeyPressed;
            Window.Closed += (s, a) => Window.Close();

            var game = new Game(Window);
            
            while(Window.IsOpen)
            {
                Window.DispatchEvents();
                game.Run();
            }

            game.Dispose();
        }

        private static void Window_KeyPressed(object sender, KeyEventArgs e)
        {
            if(e.Code == Keyboard.Key.Escape)
            {
                Window.Close();
            }
        }
    }


}