using ASTROMARINES.Other;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace AstroMarinesTests
{
    [TestClass]
    public class ExplosionTests
    {
        private RenderWindow window;
        private ExplosionFactory explosionFactory;

        [TestInitialize]
        public void SetUp()
        {
            var videoMode = new VideoMode(0, 0);
            window =  new RenderWindow(videoMode, "Test");
            explosionFactory = new ExplosionFactory();
        }

        [TestMethod]
        public void Should_Throw_OutOfRangeException_When_Try_To_Access_NonExisting_Frame()
        {
            //arrange
            var explosionPosition = new Vector2f(0, 0);
            var explosion = explosionFactory.CreateExplosion(explosionPosition);
            
            //act
            do
            {
                explosion.Draw(window);                         //drawing... 
            } while (explosion.ShouldBeDeleted.Equals(false));           //as long as it has animation frames

            //assert
            try
            {
                explosion.Draw(window);                         //if this doesn't throw exception
                Assert.Fail();                                           //it should fail
            }
            catch (System.ArgumentOutOfRangeException) { }
        }
    }
}
