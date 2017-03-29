using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Window;
using SFML.Graphics;
using SFML.System;
using SFML.Audio;
using ASTROMARINES.Characters.Enemies;
using System.Threading;
using ASTROMARINES.Properties;
using ASTROMARINES.Levels;
using ASTROMARINES.Other;

namespace ASTROMARINES
{
    public class Program
    {
        public static RenderWindow window;

        static void Main(string[] args)
        {
            window = new RenderWindow(new VideoMode(1000, 580), "dsada");
            WindowProperties.WindowWidth = window.Size.X;
            WindowProperties.WindowHeight = window.Size.Y;
            window.SetFramerateLimit(60);
            window.SetMouseCursorVisible(false);
            window.SetVerticalSyncEnabled(true);

            Game game = new Game(window);
            
            window.Closed += (s, a) => window.Close();
            while(window.IsOpen)
            {
                window.DispatchEvents();
                if (Keyboard.IsKeyPressed(Keyboard.Key.Escape))
                    window.Close();

                game.Run();
            }
        }

    }


}