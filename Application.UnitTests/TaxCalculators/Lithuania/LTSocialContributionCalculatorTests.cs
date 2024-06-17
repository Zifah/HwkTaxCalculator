using Application.TaxCalculators.Default;

namespace Application.UnitTests.TaxCalculators.Lithuania
{
    [TestFixture]
    public class LTSocialContributionCalculatorTests : LTTaxCalculatorTests
    {
        public LTSocialContributionCalculatorTests() : base("LT:SocialContribution")
        {
            _calculator = new LTSocialContributionCalculator(_mockConfigurationFactory.Object, _mockDeductibleFactory.Object);
        }

        [Test]
        public void TaxFieldName_ShouldReturn_IncomeTax()
        {
            Assert.That(_calculator.TaxFieldName, Is.EqualTo("SocialTax"));
        }
    }
}
