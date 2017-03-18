using Microsoft.VisualStudio.TestTools.UnitTesting;
using ASTROMARINES;
using SFML.Graphics;
using SFML.Window;
using SFML.System;

namespace AstroMarinesTests
{
    [TestClass]
    public class ExplosionTests
    {
        RenderWindow window;
        ExplosionFactory explosionFactory;

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
                explosion.DrawExplosion(window);                         //drawing as long as it has animation frames
            } while (explosion.ShouldBeDeleted.Equals(false));

            //assert
            try
            {
                explosion.DrawExplosion(window);                         //if this doesn't throw exception
                Assert.Fail();                                           //it should fail
            }
            catch (System.ArgumentOutOfRangeException ex) { }
        }
    }
}
