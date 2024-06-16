using Application.TaxCalculators;
using Core;
using Core.Deductibles;
using Core.Dto;

namespace Application.Deductibles
{
    public class DefaultCharityDeductibleCalculator : IDeductibleCalculator
    {
        public string[] ApplicableTaxes { get; }
        private readonly decimal _maxPercentage;
        public string UniqueName => "Default: CharityDeductible";

        public DefaultCharityDeductibleCalculator(IConfigProvider configProvider)
        {
            ApplicableTaxes = configProvider.GetValue<string[]>("ApplicableTaxes");
            _maxPercentage = configProvider.GetValue<decimal>("MaxPercentage");
        }

        public decimal Calculate(TaxPayer taxPayer)
        {
            var maximumDeductibleAmount = decimal.Round(_maxPercentage * taxPayer.GrossIncome, 2);
            return Math.Min(taxPayer.CharitySpent, maximumDeductibleAmount);
        }
    }
}
