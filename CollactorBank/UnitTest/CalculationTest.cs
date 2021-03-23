using DAL;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace UnitTest
{
    [TestClass]
    public class CalculationTest
    {

        [TestMethod]
        public void CalculatTollIsSuccess()
        {
            var calc = new TollCalculator();
            var act = calc.GetTollFee(new Car() { RegNr="XYZ123"},MokTime.GetPassTime().ToArray());
            Assert.AreEqual(act, 18);
        }
    }
}
