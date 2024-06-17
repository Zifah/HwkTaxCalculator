using Application.TaxCalculators.Default;
using Core.Configuration;
using Core.Configuration.Parameters;
using Core.Deductibles;
using Core.Dto;
using Moq;

namespace Application.UnitTests.TaxCalculators.Default
{
    [TestFixture]
    public class DefaultIncomeTaxCalculatorTests
    {
        private Mock<IConfigurationFactory> _mockConfigurationFactory;
        private Mock<IDeductibleFactory> _mockDeductibleFactory;
        private Mock<IConfigProvider> _mockConfigProvider;
        private Mock<IDeductibleCalculator> _mockDeductibleCalculatorB;
        private Mock<IDeductibleCalculator> _mockDeductibleCalculatorA;
        private BaseTaxParameters _baseTaxParameters;
        private DefaultIncomeTaxCalculator _calculator;

        [SetUp]
        public void SetUp()
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

            _mockConfigurationFactory.Setup(cf => cf.GetConfigProvider("Default:IncomeTax"))
                .Returns(_mockConfigProvider.Object);

            _mockDeductibleFactory.Setup(df => df.GetCalculators("Default:IncomeTax"))
                .Returns(new[] { _mockDeductibleCalculatorA.Object, _mockDeductibleCalculatorB.Object });

            _calculator = new DefaultIncomeTaxCalculator(
                _mockConfigurationFactory.Object,
                _mockDeductibleFactory.Object);
        }

        [Test]
        public void TaxFieldName_ShouldReturn_IncomeTax()
        {
            Assert.That(_calculator.TaxFieldName, Is.EqualTo("IncomeTax"));
        }

        [Test]
        public void UniqueName_ShouldReturn_DefaultIncomeTax()
        {
            Assert.That(_calculator.UniqueName, Is.EqualTo("Default:IncomeTax"));
        }

        [Test]
        public void IsApplicableTo_ShouldReturnTrue_ForNumericSSN()
        {
            var taxPayer = new TaxPayer { SSN = "123456789" };
            Assert.IsTrue(_calculator.IsApplicableTo(taxPayer));
        }

        [Test]
        public void IsApplicableTo_ShouldReturnFalse_ForNonNumericSSN()
        {
            var taxPayer = new TaxPayer { SSN = "ABC123456" };
            Assert.IsFalse(_calculator.IsApplicableTo(taxPayer));
        }

        [TestCase("Irina", "123456789", 3400, 0, 240)]
        [TestCase("George", "987654321", 980, 0, 0)]
        [TestCase("Mick", "321987654", 2500, 150, 135)]
        [TestCase("Bill", "219876543", 3600, 360, 224)]
        public void Calculate_ShouldReturnCorrectTaxAmount(string fullName, string ssn, decimal grossIncome, decimal deductible,
            decimal expectedTax)
        {
            var taxPayer = new TaxPayer { FullName = fullName, SSN = ssn, GrossIncome = grossIncome };
            _mockDeductibleCalculatorA.Setup(dc => dc.Calculate(taxPayer)).Returns(deductible);

            var result = _calculator.Calculate(taxPayer);

            Assert.That(result, Is.EqualTo(expectedTax));
        }

        public void Calculate_ShouldAddUpMultipleDeductibles()
        {
            decimal deductibleA = 50, deductibleB = 100, expectedTax = 135;
            var taxPayer = new TaxPayer { FullName = "Mick", SSN = "321987654", GrossIncome = 2500 };

            _mockDeductibleCalculatorA.Setup(dc => dc.Calculate(taxPayer)).Returns(deductibleA);
            _mockDeductibleCalculatorA.Setup(dc => dc.Calculate(taxPayer)).Returns(deductibleB);


            _mockDeductibleFactory.Setup(df => df.GetCalculators("Default:IncomeTax"))
                .Returns(new[] { _mockDeductibleCalculatorA.Object });

            var result = _calculator.Calculate(taxPayer);

            Assert.That(result, Is.EqualTo(expectedTax));
        }

        [Test]
        public void Calculate_ShouldReturnZero_ForNonApplicableTaxPayer()
        {
            var taxPayer = new TaxPayer { SSN = "ABC123456", GrossIncome = 50000m };

            var result = _calculator.Calculate(taxPayer);

            Assert.That(result, Is.EqualTo(0m));
        }
    }
}
