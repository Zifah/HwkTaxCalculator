using Core.Configuration;
using Core.Configuration.Parameters;
using Core.Deductibles;
using Core.Dto;
using Core.TaxCalculators;

namespace Application.TaxCalculators
{
    public abstract class TaxCalculatorBase : ITaxCalculator
    {
        public abstract string TaxFieldName { get; }

        public abstract string UniqueName { get; }

        protected readonly DefaultTaxParameters _taxParameters;
        protected readonly IDeductibleCalculator[] _deductibleCalculators;

        public TaxCalculatorBase(
            IConfigurationFactory configurationFactory,
            IDeductibleFactory deductibleFactory)
        {
            var configProvider = configurationFactory.GetConfigProvider(UniqueName);
            _taxParameters = configProvider.Get<DefaultTaxParameters>()!;
            _deductibleCalculators = deductibleFactory.GetCalculators(UniqueName);
        }

        public decimal Calculate(TaxPayer taxPayer)
        {
            if (!IsApplicableTo(taxPayer))
            {
                return 0;
            }

            var taxableIncome = taxPayer.GrossIncome - CalculateDeductions(taxPayer);
            taxableIncome = Math.Min(taxableIncome, _taxParameters.MaximumTaxableAmount ?? decimal.MaxValue);
            taxableIncome -= _taxParameters.TaxFreeAmount;

            taxableIncome = Math.Max(0, taxableIncome); // No negative taxes.

            return decimal.Round(taxableIncome * _taxParameters.Percentage / 100, 2);
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
