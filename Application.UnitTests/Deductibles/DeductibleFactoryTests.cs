using Application.Deductibles;
using Core.Deductibles;
using Moq;

namespace Application.UnitTests.Deductibles
{
    [TestFixture]
    public class DeductibleFactoryTests
    {
        [Test]
        public void GetCalculators_ShouldReturnCalculatorsWithMatchingTaxName()
        {
            // Arrange
            var taxName = "TaxA";

            var calculatorMock1 = new Mock<IDeductibleCalculator>();
            calculatorMock1.Setup(c => c.ApplicableTaxes).Returns(new string[] { "TaxA", "TaxB" });

            var calculatorMock2 = new Mock<IDeductibleCalculator>();
            calculatorMock2.Setup(c => c.ApplicableTaxes).Returns(new string[] { "TaxB" });

            var calculatorMock3 = new Mock<IDeductibleCalculator>();
            calculatorMock3.Setup(c => c.ApplicableTaxes).Returns(new string[] { "TaxA" });

            var calculators = new List<IDeductibleCalculator> { calculatorMock1.Object, calculatorMock2.Object, calculatorMock3.Object };
            var factory = new DeductibleFactory(calculators);

            // Act
            var result = factory.GetCalculators(taxName);

            // Assert
            Assert.That(result.Length, Is.EqualTo(2)); 
            Assert.That(result, Is.EquivalentTo(new[] { calculatorMock1.Object, calculatorMock3.Object }));
        }

        [Test]
        public void GetCalculators_ShouldReturnEmptyArrayWhenNoCalculatorsMatchTaxName()
        {
            // Arrange
            var taxName = "TaxC";

            var calculatorMock1 = new Mock<IDeductibleCalculator>();
            calculatorMock1.Setup(c => c.ApplicableTaxes).Returns(new string[] { "TaxA", "TaxB" });

            var calculatorMock2 = new Mock<IDeductibleCalculator>();
            calculatorMock2.Setup(c => c.ApplicableTaxes).Returns(new string[] { "TaxB" });

            var calculators = new List<IDeductibleCalculator> { calculatorMock1.Object, calculatorMock2.Object };
            var factory = new DeductibleFactory(calculators);

            // Act
            var result = factory.GetCalculators(taxName);

            // Assert
            Assert.That(result.Length, Is.EqualTo(0));
        }

        [Test]
        public void GetCalculators_ShouldReturnEmptyArrayWhenNoCalculatorsProvided()
        {
            // Arrange
            var taxName = "TaxA";
            var factory = new DeductibleFactory(new List<IDeductibleCalculator>());

            // Act
            var result = factory.GetCalculators(taxName);

            // Assert
            Assert.That(result.Length, Is.EqualTo(0));
        }
    }
}
