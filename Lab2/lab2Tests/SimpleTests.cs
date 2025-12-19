using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace lab2Tests
{
    [TestClass]
    public class SimpleTests
    {
        [TestMethod]
        public void OnePlusOneEqualsTwo()
        {
            Assert.AreEqual(2, 1 + 1);
        }
        
        [TestMethod]
        public void TrueIsTrue()
        {
            Assert.IsTrue(true);
        }
    }
}
