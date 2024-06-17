using Core.Configuration;
using Core.Configuration.Parameters;
using Core.Deductibles;
using Core.Dto;

namespace Application.Deductibles.Default
{
    public class DefaultCharityDeductibleCalculator : IDeductibleCalculator
    {
        public string[] ApplicableTaxes { get; }
        private readonly decimal _maxPercentage;
        public string UniqueName => "Default:CharityDeductible";

        public DefaultCharityDeductibleCalculator(IConfigurationFactory configurationFactory)
        {
            var configProvider = configurationFactory.GetConfigProvider(UniqueName);

            var configuration = configProvider.Get<DefaultCharityDeductibleParameters>();
            ApplicableTaxes = configuration.ApplicableTaxes;
            _maxPercentage = configuration.MaxPercentage;
        }

        public decimal Calculate(TaxPayer taxPayer)
        {
            var maximumDeductibleAmount = _maxPercentage * taxPayer.GrossIncome / 100;
            return Math.Min(taxPayer.CharitySpent, maximumDeductibleAmount);
        }
    }
}
