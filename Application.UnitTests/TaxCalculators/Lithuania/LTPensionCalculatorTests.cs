using Application.TaxCalculators.Lithuania;

namespace Application.UnitTests.TaxCalculators.Lithuania
{
    [TestFixture]
    public class LTPensionCalculatorTests : LTTaxCalculatorTests
    {
        public LTPensionCalculatorTests() : base("LT:Pension")
        {
            _calculator = new LTPensionCalculator(_mockConfigurationFactory.Object, _mockDeductibleFactory.Object);
        }

        [Test]
        public void TaxFieldName_ShouldReturn_IncomeTax()
        {
            Assert.That(_calculator.TaxFieldName, Is.EqualTo("Pension"));
        }
    }
}
