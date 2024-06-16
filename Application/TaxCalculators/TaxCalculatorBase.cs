using Core.Configuration;
using Core.Deductibles;
using Core.Dto;
using Core.TaxCalculators;

namespace Application.TaxCalculators
{
    public abstract class TaxCalculatorBase : ITaxCalculator
    {
        public abstract string TaxFieldName { get; }

        public abstract string UniqueName { get; }

        protected readonly decimal _percentage;
        protected readonly decimal _taxFreeAmount;
        protected readonly decimal? _maximumTaxableAmount;
        protected readonly IDeductibleCalculator[] _deductibleCalculators;

        public TaxCalculatorBase(
            IConfigurationFactory configurationFactory,
            IDeductibleFactory deductibleFactory)
        {
            var configProvider = configurationFactory.GetConfigProvider(UniqueName);

            _percentage = configProvider.GetValue<decimal>("Percentage");
            _taxFreeAmount = configProvider.GetValue<decimal>("TaxFreeAmount");
            _maximumTaxableAmount = configProvider.GetValue<decimal?>("MaximumTaxableAmount");

            _deductibleCalculators = deductibleFactory.GetCalculators(UniqueName);
        }

        public decimal Calculate(TaxPayer taxPayer)
        {
            if (!IsApplicableTo(taxPayer))
            {
                return 0;
            }

            var taxableIncome = taxPayer.GrossIncome - CalculateDeductions(taxPayer);
            taxableIncome = Math.Min(taxableIncome, _maximumTaxableAmount ?? decimal.MaxValue);
            taxableIncome -= _taxFreeAmount;

            taxableIncome = Math.Max(0, taxableIncome); // No negative taxes.

            return decimal.Round(taxableIncome * _percentage / 100, 2);
        }

        private decimal CalculateDeductions(TaxPayer taxPayer)
        {
            decimal totalDeduction = 0;
            foreach (var calculator in _deductibleCalculators)
            {
                totalDeduction += calculator.Calculate(taxPayer);
            }
            return totalDeduction;
        }

        public abstract bool IsApplicableTo(TaxPayer taxPayer);
    }
}
