using ASTROMARINES.Characters.Player;
using ASTROMARINES.Other;
using ASTROMARINES.Properties;
using SFML.Graphics;
using System;
using System.Collections.Generic;

namespace ASTROMARINES.Levels
{
    class Game
    {
        IPlayer player;
        Queue<Tuple<string,string>> levelNamesQueue;
        ILevel currentLevel;
        Menu menu;
        RenderWindow window;

        public Game(RenderWindow window)
        {
            player = new Player();
            levelNamesQueue = new Queue<Tuple<string, string>>();
            currentLevel = new SimpleImageScreen(Resources.TitleBG);
            menu = new Menu();
            this.window = window;
        }

        public void Run()
        {
            if(currentLevel.HasLevelEnded)
            {
                if (levelNamesQueue.Count == 0)
                {
                    levelNamesQueue = menu.MenuLogic();
                    menu.Draw(window);
                }
                else
                {
                    currentLevel.Dispose();
                    var levelNameAndArg = levelNamesQueue.Dequeue();
                    var levelType = Type.GetType($"ASTROMARINES.Levels.{levelNameAndArg.Item1}");

                    if (levelNameAndArg.Item2.Equals("SendPlayerAsArgument"))
                        currentLevel = (ILevel)Activator.CreateInstance(levelType, player);
                    else
                        currentLevel = (ILevel)Activator.CreateInstance(levelType, levelNameAndArg.Item2);
                }
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