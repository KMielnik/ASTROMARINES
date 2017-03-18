using System;
using System.Collections.Generic;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace ASTROMARINES
{
    public partial class Player : IPlayer
    {
        class Weapon
        {
            List<RectangleShape> Cannons;

            public Weapon()
            {
                var Cannon = new RectangleShape(new Vector2f(3, 20));
                Cannon.Origin = new Vector2f(1.5f, 20);
                Cannon.FillColor = new Color(Color.White);
                Cannon.OutlineThickness = 2;
                Cannon.OutlineColor = new Color(Color.Black);
                Cannons.Add(Cannon);

                for(int i=0;i<4;i++)
                {
                    var NextCannon = new RectangleShape(Cannon);
                    Cannons.Add(NextCannon);
                }
            }

            void SetWeaponPosition(Vector2f playerPosition, Vector2f playerDimensions, RenderWindow window)
            {
                Vector2f CannonOrigin = playerPosition;
                CannonOrigin += new Vector2f(0, -playerDimensions.Y / 6);
                Cannons[0].Position = CannonOrigin;

                CannonOrigin = playerPosition;
                CannonOrigin += new Vector2f(playerDimensions.X / 2.8f, -playerDimensions.Y / 3);
                Cannons[1].Position = CannonOrigin;

                CannonOrigin = playerPosition;
                CannonOrigin += new Vector2f(playerDimensions.X / 2.8f, playerDimensions.Y / 3);
                Cannons[2].Position = CannonOrigin;

                CannonOrigin = playerPosition;
                CannonOrigin += new Vector2f(-playerDimensions.X / 3.7f, -playerDimensions.Y / 3.5f);
                Cannons[3].Position = CannonOrigin;

                CannonOrigin = playerPosition;
                CannonOrigin += new Vector2f(-playerDimensions.X / 3.7f, playerDimensions.Y / 3.5f);
                Cannons[4].Position = CannonOrigin;

                var angle = CalculateAngle(playerPosition, playerDimensions, window);
                foreach (var cannon in Cannons)
                    cannon.Rotation = angle;
            }

            float CalculateAngle(Vector2f playerPosition, Vector2f playerDimensions, RenderWindow window)
            {
                float x;
                float y;
                var MousePosition = Mouse.GetPosition(window);
                x = MousePosition.X - playerPosition.X;
                y = playerPosition.Y - MousePosition.Y - playerDimensions.Y / 6;

                return (float)(Math.Atan2(y, x) / (2 * Math.PI)) * 360;
            }

            void Draw(RenderWindow window, PlayerLevel playerLevel)
            {
                switch(playerLevel)
                {
                    case PlayerLevel.Level1:
                    case PlayerLevel.Level2:
                        window.Draw(Cannons[0]);
                        break;
                    case PlayerLevel.Level3:
                        for (int i = 0; i < 3; i++)
                            window.Draw(Cannons[i]);
                        break;
                    case PlayerLevel.Level4:
                        foreach (var cannon in Cannons)
                            window.Draw(cannon);
                        break;
                }
            }
        }
    }
}
