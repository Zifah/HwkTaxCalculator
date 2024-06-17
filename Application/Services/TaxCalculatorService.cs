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
            var cacheKey = $"Taxes_{taxPayer.SSN}";
            var taxes = _cacheService.RetrieveItem<Taxes>(cacheKey);
            if (taxes != null)
            {
                return taxes;
            }

            var taxDictionary = new Dictionary<string, decimal>();

            foreach (var taxCalculator in _taxCalculators.Where(tc => tc.IsApplicableTo(taxPayer)))
            {
                var taxFieldName = taxCalculator.TaxFieldName;
                ValidateTaxNotCalculated(taxPayer, taxDictionary, taxFieldName);
                taxDictionary[taxFieldName] = taxCalculator.Calculate(taxPayer);
            }

            if (!taxDictionary.Any())
            {
                throw new BadRequestException("Unable to calculate taxes for this taxpayer's jurisdiction.");
            }

            taxes = new Taxes(taxDictionary)
            {
                GrossIncome = taxPayer.GrossIncome,
                CharitySpent = taxPayer.CharitySpent
            };

            _cacheService.CacheItem(cacheKey, taxes);

            return taxes;
        }

        private static void ValidateTaxNotCalculated(TaxPayer taxPayer, Dictionary<string, decimal> taxDictionary, string taxFieldName)
        {
            if (taxDictionary.ContainsKey(taxFieldName))
            {
                string message = $"More than one {taxFieldName} calculation exists for taxpayer {taxPayer.SSN}.";
                throw new ConfigurationException(message);
            }
        }
    }
}
