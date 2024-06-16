using Core.Configuration;
using Core.Dto;
using Core.Exceptions;
using Core.Services;
using Core.TaxCalculators;

namespace Application.Services
{
    public class TaxCalculatorService : ITaxCalculatorService
    {
        private readonly IEnumerable<ITaxCalculator> _taxCalculators;
        private readonly ICacheService _cacheService;

        public TaxCalculatorService(
            IEnumerable<ITaxCalculator> taxCalculators,
            ICacheService cacheService)
        {
            _taxCalculators = taxCalculators;
            _cacheService = cacheService;
        }

        public Taxes CalculateTaxes(TaxPayer taxPayer)
        {
            var taxes = _cacheService.RetrieveItem<Taxes>($"Taxes_{taxPayer.SSN}");
            if (taxes != null)
            {
                return taxes;
            }

            var taxDictionary = new Dictionary<string, decimal>();

            foreach (var taxCalculator in _taxCalculators.Where(tc => tc.IsApplicableTo(taxPayer)))
            {
                var taxFieldName = taxCalculator.TaxFieldName;

                if (taxDictionary.ContainsKey(taxFieldName))
                {
                    string message = $"More than one {taxFieldName} tax calculation exists  for taxpayer {taxPayer.SSN}.";
                    throw new ConfigurationException(message);
                }

                taxDictionary[taxFieldName] = taxCalculator.Calculate(taxPayer);
            }

            // TODO: Cache taxes before returning.
            return new Taxes(taxDictionary)
            {
                GrossIncome = taxPayer.GrossIncome,
                CharitySpent = taxPayer.CharitySpent
            };
        }
    }
}
