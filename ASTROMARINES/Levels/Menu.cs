using ASTROMARINES.Other;
using ASTROMARINES.Properties;
using SFML.Graphics;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Threading;

namespace ASTROMARINES.Levels
{
    partial class Menu : IDisposable
    {
        MousePointer mousePointer;
        Texture backgroundTexture;
        Sprite background;
        List<Button> buttons;
        
        public Menu()
        {
            mousePointer = new MousePointer();
            backgroundTexture = new Texture(Resources.MenuBG);
            background = new Sprite(backgroundTexture);
            buttons = new List<Button>();
        }

        /// <summary>
        /// Performs menu logic, and tries to return choosen
        /// levelQueue, if not choosen returns empty queue to give another frame in menu
        /// </summary>
        /// <returns></returns>
        public Queue<Tuple<string, string>> MenuLogic()
        {
            Queue<Tuple<string, string>> elo = new Queue<Tuple<string, string>>();
            return elo;
        }

        public void Draw(RenderWindow window)
        {
            window.Clear(Color.Black);

            window.Draw(background);
            foreach (var button in buttons)
                button.Draw(window);
            mousePointer.Draw(window);

            window.Display();
        }

        public void Dispose()
        {
            mousePointer.Dispose();
            backgroundTexture.Dispose();
            background.Dispose();
            foreach (var button in buttons)
                button.Dispose();
        }
    }
}