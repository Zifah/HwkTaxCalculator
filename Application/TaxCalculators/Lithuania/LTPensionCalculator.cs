using Core.Configuration;
using Core.Deductibles;
using Core.Dto;

namespace Application.TaxCalculators.Lithuania
{
    public class LTPensionCalculator : LTTaxCalculatorBase
    {
        public override string TaxFieldName => nameof(Taxes.Pension);
        public override string UniqueName => "LT:Pension";

        public LTPensionCalculator(IConfigurationFactory configurationFactory, IDeductibleFactory deductibleFactory) :
            base(configurationFactory, deductibleFactory)
        {
        }
    }
}
