using DAL;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace UnitTest
{
    [TestClass]
    public class DomainTest
    {
        
        [TestMethod]
        public void GetVeicleTypeIsSuccess()
        {
            var act = new Car();
            var exp = "Car";

            Assert.AreEqual(exp, act.GetVehicleType().ToString());
        }

        [TestMethod]
        public void IsTollFreeVehicleTest()
        {
            var tc = new TollCalculator();
            var act = new Motorbike();

            Assert.IsTrue(tc.IsTollFreeVehicle(act));
        }

    }
}
