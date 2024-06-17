using Application.Deductibles.Default;
using Core.Configuration;
using Core.Configuration.Parameters;
using Core.Dto;
using Moq;

namespace Application.UnitTests.Deductibles.Default
{
    public class DefaultCharityDeductibleCalculatorTests
    {
        private const string UniqueName = "Default:CharityDeductible";

        private readonly Mock<IConfigurationFactory> _configurationFactoryMock;
        private readonly Mock<IConfigProvider> _configProviderMock;
        private readonly DefaultCharityDeductibleCalculator _calculator;

        public DefaultCharityDeductibleCalculatorTests()
        {
            _configurationFactoryMock = new Mock<IConfigurationFactory>();
            _configProviderMock = new Mock<IConfigProvider>();

            var configParameters = new DefaultCharityDeductibleParameters(Array.Empty<string>(), 10m);

            _configProviderMock.Setup(cp => cp.Get<DefaultCharityDeductibleParameters>())
                .Returns(configParameters);

            _configurationFactoryMock.Setup(cf => cf.GetConfigProvider(UniqueName))
                .Returns(_configProviderMock.Object);

            _calculator = new DefaultCharityDeductibleCalculator(_configurationFactoryMock.Object);
        }

        [Test]
        public void UniqueName_ShouldReturn_DefaultCharityDeductible()
        {
            Assert.That(_calculator.UniqueName, Is.EqualTo(UniqueName));
        }

        [Test]
        public void Calculate_ShouldReturnCorrectDeductibleAmount_WhenCharitySpentIsLessThanMaxDeductible()
        {
            // Arrange
            var taxPayer = new TaxPayer
            {
                GrossIncome = 2500,
                CharitySpent = 150
            };

            // Act
            var result = _calculator.Calculate(taxPayer);

            // Assert
            Assert.That(result, Is.EqualTo(150));
        }

        [Test]
        public void Calculate_ShouldReturnCorrectDeductibleAmount_WhenCharitySpentIsEqualToMaxDeductible()
        {
            // Arrange
            var taxPayer = new TaxPayer
            {
                GrossIncome = 3600,
                CharitySpent = 360
            };

            // Act
            var result = _calculator.Calculate(taxPayer);

            // Assert
            Assert.That(result, Is.EqualTo(360));
        }

        [Test]
        public void Calculate_ShouldReturnCorrectDeductibleAmount_WhenCharitySpentIsGreaterThanMaxDeductible()
        {
            // Arrange
            var taxPayer = new TaxPayer
            {
                GrossIncome = 3600,
                CharitySpent = 520
            };

            // Act
            var result = _calculator.Calculate(taxPayer);

            // Assert
            Assert.That(result, Is.EqualTo(360));
        }

        [Test]
        public void Calculate_ShouldReturnZero_WhenCharitySpentIsZero()
        {
            // Arrange
            var taxPayer = new TaxPayer
            {
                GrossIncome = 3600,
                CharitySpent = 0m
            };

            // Act
            var result = _calculator.Calculate(taxPayer);

            // Assert
            Assert.That(result, Is.EqualTo(0m));
        }

        [Test]
        public void Calculate_ShouldReturnZero_WhenGrossIncomeIsZero()
        {
            // Arrange
            var taxPayer = new TaxPayer
            {
                GrossIncome = 0m,
                CharitySpent = 100
            };

            // Act
            var result = _calculator.Calculate(taxPayer);

            // Assert
            Assert.That(result, Is.EqualTo(0m));
        }
    }

}
