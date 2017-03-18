using System;
using ASTROMARINES;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using SFML.Audio;
using SFML.System;
using SFML.Graphics;
using SFML.Window;

namespace AstroMarinesTests
{
    [TestClass]
    public class BulletTests
    {
        [TestMethod]
        public void Should_Set_Bullet_For_Deletion_If_It_Flew_Out_Of_Map()
        {
            //arrange
            Vector2f[] positions = new Vector2f[5];
            positions[0] = new Vector2f(-100, 0);                                       //out of the map
            positions[1] = new Vector2f(WindowProperties.WindowWidth + 100, 0);         //out of the map
            positions[2] = new Vector2f(0, -100);                                       //out of the map
            positions[3] = new Vector2f(0, WindowProperties.WindowHeight + 100);        //out of the map
            positions[4] = new Vector2f(100, WindowProperties.WindowHeight / 2);        //in the map

            Vector2f vector = new Vector2f(0, 0);

            Bullet[] bullets = new Bullet[5];

            for (int i = 0; i < positions.Length; i++)
            {
                bullets[i] = new Bullet(positions[i], vector);
            }

            //act
            for (int i = 0; i < bullets.Length; i++)
            {
                bullets[i].Move();
            }

            //assert
            for (int i = 0; i < positions.Length - 1; i++)
            {
                Assert.IsTrue(bullets[i].ShouldBeDeleted);
            }
            Assert.IsFalse(bullets[bullets.Length - 1].ShouldBeDeleted);
        }
    }
}
