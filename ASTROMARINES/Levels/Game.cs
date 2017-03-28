using ASTROMARINES.Characters.Player;
using ASTROMARINES.Other;
using SFML.Graphics;
using System;
using System.Collections.Generic;

namespace ASTROMARINES.Levels
{
    class Game
    {
        IPlayer player;
        Queue<string> levelNamesQueue;
        ILevel currentLevel;
        Menu menu;
        RenderWindow window;

        public Game(RenderWindow window)
        {
            player = new Player();
            levelNamesQueue = new Queue<string>();
            currentLevel = new SimpleTextScreen("XD");
            menu = new Menu();
            this.window = window;
        }

        public void Run()
        {
            if (levelNamesQueue.Count == 0 && currentLevel.HasLevelEnded)
            {
                levelNamesQueue = menu.MenuLogic();
                menu.Draw(window);
            }
            else if(currentLevel.HasLevelEnded)
            {
                string levelName = levelNamesQueue.Dequeue();
                var levelType = Type.GetType($"ASTROMARINES.Levels.{levelName}");
                currentLevel = (ILevel)Activator.CreateInstance(levelType,"XdasdD");
            }
            else
            {
                currentLevel.LevelLogic(window);
                currentLevel.Draw(window);
            }

            if(player.ShouldBeDeleted)
            {
                player = new Player();
                currentLevel = null;
                levelNamesQueue.Clear();
            }
        }
    }
}