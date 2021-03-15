using NUnit.Framework;
using System;
using TaxCalculatorInterviewTests;

namespace TaxCalculatorNUnitTest
{
    public class Tests
    {
        TaxCalculator _taxCalculator;

        [SetUp]
        public void Setup()
        {
            _taxCalculator = new TaxCalculator();
        }

        [Test]
        public void Test1()
        {

            _taxCalculator.SetCustomTaxRate(Commodity.Default, 0.25);
            _taxCalculator.SetCustomTaxRate(Commodity.Default, 0.50);
            Assert.Pass();
        }
    }
}