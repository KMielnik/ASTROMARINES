using System;
using System.Collections.Generic;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using ASTROMARINES.Other;

namespace ASTROMARINES.Characters.Player
{
    partial class Player : IPlayer
    {
        class Weapon : IDisposable
        {
            readonly List<RectangleShape> _cannons;
            readonly Clock _reloadClock;

            public Weapon()
            {
                _cannons = new List<RectangleShape>();

                var cannon = new RectangleShape(new Vector2f(3, 20));
                cannon.Origin = new Vector2f(1.5f, 20);
                cannon.FillColor = new Color(Color.White);
                cannon.OutlineThickness = 2;
                cannon.OutlineColor = new Color(Color.Black);
                _cannons.Add(cannon);

                for(var i=0;i<4;i++)
                {
                    var nextCannon = new RectangleShape(cannon);
                    _cannons.Add(nextCannon);
                }

                _reloadClock = new Clock();
            }

            public void SetWeaponPosition(Vector2f playerPosition, Vector2f playerDimensions, RenderWindow window)
            {
                var cannonOrigin = playerPosition;
                cannonOrigin += new Vector2f(0, -playerDimensions.Y / 6);
                _cannons[0].Position = cannonOrigin;

                cannonOrigin = playerPosition;
                cannonOrigin += new Vector2f(playerDimensions.X / 2.8f, -playerDimensions.Y / 3);
                _cannons[1].Position = cannonOrigin;

                cannonOrigin = playerPosition;
                cannonOrigin += new Vector2f(-playerDimensions.X / 3.7f, -playerDimensions.Y / 3.5f);
                _cannons[2].Position = cannonOrigin;

                cannonOrigin = playerPosition;
                cannonOrigin += new Vector2f(playerDimensions.X / 2.8f, playerDimensions.Y / 3);
                _cannons[3].Position = cannonOrigin;

                cannonOrigin = playerPosition;
                cannonOrigin += new Vector2f(-playerDimensions.X / 3.7f, playerDimensions.Y / 3.5f);
                _cannons[4].Position = cannonOrigin;

                var angle = CalculateAngle(playerPosition, playerDimensions, window);
                foreach (var cannon in _cannons)
                    cannon.Rotation = angle;
            }

            float CalculateAngle(Vector2f playerPosition, Vector2f playerDimensions, RenderWindow window)
            {
                float x;
                float y;
                var mousePosition = Mouse.GetPosition(window);
                x = mousePosition.X - playerPosition.X;
                y = playerPosition.Y - mousePosition.Y - playerDimensions.Y / 6;

                return (float)(Math.Atan2(x, y) / (2 * Math.PI)) * 360;
            }

            public void Draw(RenderWindow window, PlayerLevel playerLevel)
            {
                switch(playerLevel)
                {
                    case PlayerLevel.Level1:
                    case PlayerLevel.Level2:
                        window.Draw(_cannons[0]);
                        break;
                    case PlayerLevel.Level3:
                        for (var i = 0; i < 3; i++)
                            window.Draw(_cannons[i]);
                        break;
                    case PlayerLevel.Level4:
                        foreach (var cannon in _cannons)
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

                var vector = CalculateNew(playerPosition, playerDimensions, window);

                switch (playerLevel)
                {
                    case PlayerLevel.Level1:
                        if (_reloadClock.ElapsedTime.AsMilliseconds() > 200)
                        {
                            newBullets.Add(new Bullet(_cannons[0].Position + vector, vector / 2));
                            _reloadClock.Restart();
                        }
                        break;
                    case PlayerLevel.Level2:
                        if (_reloadClock.ElapsedTime.AsMilliseconds() > 50)
                        {
                            newBullets.Add(new Bullet(_cannons[0].Position + vector, vector / 4));
                            _reloadClock.Restart();
                        }
                        break;
                    case PlayerLevel.Level3:
                        if (_reloadClock.ElapsedTime.AsMilliseconds() > 150)
                        {
                            newBullets.Add(new Bullet(_cannons[0].Position + vector, vector / 1));
                            newBullets.Add(new Bullet(_cannons[1].Position + vector, vector / 1));
                            newBullets.Add(new Bullet(_cannons[2].Position + vector, vector / 1));
                            _reloadClock.Restart();
                        }
                        break;
                    case PlayerLevel.Level4:
                        if (_reloadClock.ElapsedTime.AsMilliseconds() > 100)
                        {
                            newBullets.Add(new Bullet(_cannons[0].Position + vector, vector / 1));
                            newBullets.Add(new Bullet(_cannons[1].Position + vector, vector / 3));
                            newBullets.Add(new Bullet(_cannons[2].Position + vector, vector / 3));
                            newBullets.Add(new Bullet(_cannons[3].Position + vector, vector / 2));
                            newBullets.Add(new Bullet(_cannons[4].Position + vector, vector / 2));
                            _reloadClock.Restart();
                        }
                        break;
                }

                return newBullets;
            }

            Vector2f CalculateNew(Vector2f playerPosition, Vector2f playerDimensions, RenderWindow window)
            {
                var mousePosition = (Vector2f)Mouse.GetPosition(window);

                //calculate mouse position relative to player
                var x = mousePosition.X - playerPosition.X;
                var y = playerPosition.Y - mousePosition.Y - playerDimensions.Y / 6;
                var z = (float)Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2));

                //calculate speed vector relative relative to length of cannon
                var newZ = _cannons[0].Size.Y -2;
                var newX = x / (z / newZ);
                var newY = -(y / (z / newZ));

                return new Vector2f(newX, newY);
            }

            public void Dispose()
            {
                foreach (var cannon in _cannons)
                    cannon.Dispose();
                _reloadClock.Dispose();
            }
        }
    }
}
