using Core.Configuration;
using Core.Deductibles;
using Core.Dto;

namespace Application.Deductibles
{
    public class DefaultCharityDeductibleCalculator : IDeductibleCalculator
    {
        public string[] ApplicableTaxes { get; }
        private readonly decimal _maxPercentage;
        public string UniqueName => "Default:CharityDeductible";

        public DefaultCharityDeductibleCalculator(IConfigurationFactory configurationFactory)
        {
            var configProvider = configurationFactory.GetConfigProvider(UniqueName);

            ApplicableTaxes = configProvider.GetValue<string[]>("ApplicableTaxes")!;
            _maxPercentage = configProvider.GetValue<decimal>("MaxPercentage");
        }

        public decimal Calculate(TaxPayer taxPayer)
        {
            var maximumDeductibleAmount = _maxPercentage * taxPayer.GrossIncome / 100;
            return Math.Min(taxPayer.CharitySpent, maximumDeductibleAmount);
        }
    }
}
