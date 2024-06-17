using Moq;
using Application.Services;
using Core.Dto;
using Core.Exceptions;
using Core.Services;
using Core.TaxCalculators;
using System;

namespace Application.UnitTests.Services
{
    [TestFixture]
    public class TaxCalculatorServiceTests
    {
        private TaxCalculatorService _taxCalculatorService;
        private Mock<IEnumerable<ITaxCalculator>> _taxCalculatorsMock;
        private Mock<ICacheService> _cacheServiceMock;

        [SetUp]
        public void Setup()
        {
            // Initialize mocks
            _taxCalculatorsMock = new Mock<IEnumerable<ITaxCalculator>>();
            _cacheServiceMock = new Mock<ICacheService>();

            _taxCalculatorService = new TaxCalculatorService(
                _taxCalculatorsMock.Object,
                _cacheServiceMock.Object);
        }

        [TestCase(true, 500, 500)]
        [TestCase(false, 500, 0)]
        public void CalculateTaxes_ReturnsValidTaxes_FromApplicableTaxCalculators(bool isPensionApplicable, decimal pension, decimal expectedPension)
        {
            // Arrange
            var taxPayer = new TaxPayer { SSN = "123456789", GrossIncome = 50000, CharitySpent = 1000 };
            decimal incomeTax = 5000, socialTax = 1000;


            // Mock behavior for tax calculators
            var taxCalculatorMock1 = new Mock<ITaxCalculator>();
            taxCalculatorMock1.Setup(tc => tc.TaxFieldName).Returns("IncomeTax");
            taxCalculatorMock1.Setup(tc => tc.IsApplicableTo(taxPayer)).Returns(true);
            taxCalculatorMock1.Setup(tc => tc.Calculate(taxPayer)).Returns(incomeTax);

            var taxCalculatorMock2 = new Mock<ITaxCalculator>();
            taxCalculatorMock2.Setup(tc => tc.TaxFieldName).Returns("SocialTax");
            taxCalculatorMock2.Setup(tc => tc.IsApplicableTo(taxPayer)).Returns(true);
            taxCalculatorMock2.Setup(tc => tc.Calculate(taxPayer)).Returns(socialTax);

            var taxCalculatorMock3 = new Mock<ITaxCalculator>();
            taxCalculatorMock3.Setup(tc => tc.TaxFieldName).Returns("Pension");
            taxCalculatorMock3.Setup(tc => tc.IsApplicableTo(taxPayer)).Returns(isPensionApplicable);
            taxCalculatorMock3.Setup(tc => tc.Calculate(taxPayer)).Returns(pension);

            // Multiple calculators for same tax, but only one (Mock2) applies for this taxpayer, so no problem.
            var taxCalculatorMock4 = new Mock<ITaxCalculator>();
            taxCalculatorMock4.Setup(tc => tc.TaxFieldName).Returns("SocialTax");
            taxCalculatorMock4.Setup(tc => tc.IsApplicableTo(taxPayer)).Returns(false);
            taxCalculatorMock4.Setup(tc => tc.Calculate(taxPayer)).Returns(socialTax);

            var taxCalculators = new List<ITaxCalculator> {
                taxCalculatorMock1.Object,
                taxCalculatorMock2.Object,
                taxCalculatorMock3.Object,
                taxCalculatorMock4.Object
            };
            _taxCalculatorsMock.Setup(tc => tc.GetEnumerator()).Returns(taxCalculators.GetEnumerator());

            // Act
            var result = _taxCalculatorService.CalculateTaxes(taxPayer);

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.GrossIncome, Is.EqualTo(taxPayer.GrossIncome));
            Assert.That(result.CharitySpent, Is.EqualTo(taxPayer.CharitySpent));

            Assert.That(result.Pension, Is.EqualTo(expectedPension));
            Assert.That(result.IncomeTax, Is.EqualTo(incomeTax));
            Assert.That(result.SocialTax, Is.EqualTo(socialTax));

            Assert.That(result.TotalTax, Is.EqualTo(incomeTax + socialTax + expectedPension));
        }

        [Test]
        public void CalculateTaxes_ThrowsConfigurationException_WhenDuplicateTaxCalculation()
        {
            // Arrange
            var taxPayer = new TaxPayer { SSN = "123456789", GrossIncome = 50000, CharitySpent = 1000 };

            // Mock behavior for tax calculators
            var taxCalculatorMock1 = new Mock<ITaxCalculator>();
            taxCalculatorMock1.Setup(tc => tc.TaxFieldName).Returns("IncomeTax");
            taxCalculatorMock1.Setup(tc => tc.IsApplicableTo(taxPayer)).Returns(true);
            taxCalculatorMock1.Setup(tc => tc.Calculate(taxPayer)).Returns(5000);

            var taxCalculatorMock2 = new Mock<ITaxCalculator>();
            taxCalculatorMock2.Setup(tc => tc.TaxFieldName).Returns("IncomeTax"); // Same tax field name
            taxCalculatorMock2.Setup(tc => tc.IsApplicableTo(taxPayer)).Returns(true);
            taxCalculatorMock2.Setup(tc => tc.Calculate(taxPayer)).Returns(3000);

            var taxCalculatorMock3 = new Mock<ITaxCalculator>();
            taxCalculatorMock3.Setup(tc => tc.TaxFieldName).Returns("Pension");
            taxCalculatorMock3.Setup(tc => tc.IsApplicableTo(taxPayer)).Returns(true);
            taxCalculatorMock3.Setup(tc => tc.Calculate(taxPayer)).Returns(1000);

            var taxCalculators = new List<ITaxCalculator> { taxCalculatorMock1.Object, taxCalculatorMock2.Object };
            _taxCalculatorsMock.Setup(tc => tc.GetEnumerator()).Returns(taxCalculators.GetEnumerator());

            // Act & Assert
            var ex = Assert.Throws<ConfigurationException>(() => _taxCalculatorService.CalculateTaxes(taxPayer));

            // Assert exception message
            Assert.That(ex.Message, Is.EqualTo($"More than one IncomeTax calculation exists for taxpayer {taxPayer.SSN}."));
        }

        [Test]
        public void CalculateTaxes_ThrowsBadRequestException_WhenNoTaxCalculatorsApply()
        {
            // Arrange
            var taxPayer = new TaxPayer { SSN = "987654321", GrossIncome = 60000, CharitySpent = 2000 };

            // Mock behavior for tax calculators (none applicable)
            var taxCalculators = new List<ITaxCalculator>(); // Empty list
            _taxCalculatorsMock.Setup(tc => tc.GetEnumerator()).Returns(taxCalculators.GetEnumerator());

            // Act & Assert
            var ex = Assert.Throws<BadRequestException>(() => _taxCalculatorService.CalculateTaxes(taxPayer));

            // Assert exception message
            Assert.That(ex.Message, Is.EqualTo($"Unable to calculate taxes for this taxpayer's jurisdiction."));
        }
    }
}