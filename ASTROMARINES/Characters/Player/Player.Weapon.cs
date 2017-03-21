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
            Clock reloadClock;

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

                reloadClock = new Clock();
            }

            public void SetWeaponPosition(Vector2f playerPosition, Vector2f playerDimensions, RenderWindow window)
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

            public void Draw(RenderWindow window, PlayerLevel playerLevel)
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

            /// <summary>
            /// Create new bullets and return them
            /// </summary>
            /// <returns>List of new bullets</returns>
            public List<Bullet> Shoot(PlayerLevel playerLevel, Vector2f playerPosition, Vector2f playerDimensions, RenderWindow window)
            {
                var newBullets = new List<Bullet>();

                Vector2f vector = CalculateNew(playerPosition, playerDimensions, window);

                switch (playerLevel)
                {
                    case PlayerLevel.Level1:
                        if (reloadClock.ElapsedTime.AsMilliseconds() > 200)
                            newBullets.Add(new Bullet(Cannons[0].Size + vector, vector / 2));
                        break;
                    case PlayerLevel.Level2:
                        if (reloadClock.ElapsedTime.AsMilliseconds() > 50)
                            newBullets.Add(new Bullet(Cannons[0].Size + vector, vector / 4));
                        break;
                    case PlayerLevel.Level3:
                        if (reloadClock.ElapsedTime.AsMilliseconds() > 150)
                        {
                            newBullets.Add(new Bullet(Cannons[0].Size + vector, vector / 1));
                            newBullets.Add(new Bullet(Cannons[1].Size + vector, vector / 1));
                            newBullets.Add(new Bullet(Cannons[2].Size + vector, vector / 1));
                        }
                        break;
                    case PlayerLevel.Level4:
                        if (reloadClock.ElapsedTime.AsMilliseconds() > 100)
                        {
                            newBullets.Add(new Bullet(Cannons[0].Size + vector, vector / 1));
                            newBullets.Add(new Bullet(Cannons[1].Size + vector, vector / 3));
                            newBullets.Add(new Bullet(Cannons[2].Size + vector, vector / 3));
                            newBullets.Add(new Bullet(Cannons[3].Size + vector, vector / 2));
                            newBullets.Add(new Bullet(Cannons[4].Size + vector, vector / 2));
                        }
                        break;
                    default:
                        break;
                }

                return newBullets;
            }

            Vector2f CalculateNew(Vector2f playerPosition, Vector2f playerDimensions, RenderWindow window)
            {
                Vector2f mousePosition = (Vector2f)Mouse.GetPosition(window);

                //calculate mouse position relative to player
                float x = mousePosition.X - playerPosition.X;
                float y = playerPosition.Y - mousePosition.Y - playerDimensions.Y / 6;
                float z = (float)Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2));

                //calculate speed vector relative relative to length of cannon
                float newZ = Cannons[0].Size.Y + 2;
                float newX = x / (z / newZ);
                float newY = -(y / (z / newZ));

                return new Vector2f(newX, newZ);
            }
        }
    }
}
