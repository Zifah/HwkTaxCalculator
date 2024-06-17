using Application.TaxCalculators.Default;
using Application.TaxCalculators.Lithuania;
using Core.Configuration.Parameters;
using Core.Configuration;
using Core.Deductibles;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UnitTests.TaxCalculators.Lithuania
{
    [TestFixture]
    public class LTIncomeTaxCalculatorTests : LTTaxCalculatorTests
    {
        public LTIncomeTaxCalculatorTests() : base("LT:IncomeTax")
        {
            _calculator = new LTIncomeTaxCalculator(_mockConfigurationFactory.Object, _mockDeductibleFactory.Object);
        }

        [Test]
        public void TaxFieldName_ShouldReturn_IncomeTax()
        {
            Assert.That(_calculator.TaxFieldName, Is.EqualTo("IncomeTax"));
        }
    }
}
