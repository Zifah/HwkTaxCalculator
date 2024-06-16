using Core;
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
            IConfigProvider configProvider,
            IDeductibleFactory deductibleFactory)
        {
            _percentage = configProvider.GetValue<decimal>("Percentage");
            _taxFreeAmount = configProvider.GetValue<decimal>("TaxFreeAmount");
            _maximumTaxableAmount = configProvider.GetValue<decimal>("MaximumTaxableAmount");

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

            return decimal.Round(taxableIncome * _percentage / 100, 2);
        }

        private decimal CalculateDeductions(TaxPayer taxPayer)
        {
            var taxableIncome = taxPayer.GrossIncome;
            foreach (var calculator in _deductibleCalculators)
            {
                taxableIncome -= calculator.Calculate(taxPayer);
            }
            return taxableIncome;
        }

        public abstract bool IsApplicableTo(TaxPayer taxPayer);
    }
}
