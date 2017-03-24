using ASTROMARINES.Other;
using SFML.Graphics;
using System.Collections.Generic;

namespace ASTROMARINES.Levels
{
    partial class Menu
    {
        MousePointer mousePointer;
        Texture backgroundTexture;
        Texture instructionsBackgroundTexture;
        Sprite background;
        List<Button> buttons;
        
        public Menu()
        {

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
    }
}