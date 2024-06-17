using Application.TaxCalculators.Lithuania;
using Core.Configuration.Parameters;
using Core.Configuration;
using Core.Deductibles;
using Core.Dto;
using Moq;

namespace Application.UnitTests.TaxCalculators.Lithuania
{
    public abstract class LTTaxCalculatorTests
    {
        protected LTTaxCalculatorBase _calculator;
        protected Mock<IConfigurationFactory> _mockConfigurationFactory;
        protected Mock<IDeductibleFactory> _mockDeductibleFactory;
        protected Mock<IConfigProvider> _mockConfigProvider;
        protected Mock<IDeductibleCalculator> _mockDeductibleCalculatorB;
        protected Mock<IDeductibleCalculator> _mockDeductibleCalculatorA;
        protected BaseTaxParameters _baseTaxParameters;

        public LTTaxCalculatorTests(string uniqueName)
        {

            _mockConfigurationFactory = new Mock<IConfigurationFactory>();
            _mockDeductibleFactory = new Mock<IDeductibleFactory>();
            _mockDeductibleCalculatorA = new Mock<IDeductibleCalculator>();
            _mockDeductibleCalculatorB = new Mock<IDeductibleCalculator>();
            _mockConfigProvider = new Mock<IConfigProvider>();

            _baseTaxParameters = new BaseTaxParameters
            {
                // Use same parameters as in the example
                TaxFreeAmount = 1000,
                Percentage = 10
            };

            _mockConfigProvider.Setup(cp => cp.Get<BaseTaxParameters>())
                .Returns(_baseTaxParameters);

            _mockConfigurationFactory.Setup(cf => cf.GetConfigProvider(uniqueName))
                .Returns(_mockConfigProvider.Object);

            _mockDeductibleFactory.Setup(df => df.GetCalculators(uniqueName))
                .Returns(new[] { _mockDeductibleCalculatorA.Object, _mockDeductibleCalculatorB.Object });
        }

        [TestCase("LT1234567", true)]
        [TestCase("LT123456A", true)]
        [TestCase("LTA123456", true)]
        [TestCase("NG1234567", false)]
        [TestCase("ABCDEFGHI", false)]
        [TestCase("123456789", false)]

        public void IsApplicableTo_ShouldApplyOnlyToLithuanianSSN(string ssn, bool isLithuanianSSN)
        {
            var taxPayer = new TaxPayer { SSN = ssn };
            Assert.That(_calculator.IsApplicableTo(taxPayer), Is.EqualTo(isLithuanianSSN));
        }
    }
}
