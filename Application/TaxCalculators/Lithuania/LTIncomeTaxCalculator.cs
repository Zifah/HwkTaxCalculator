using Application.TaxCalculators.Lithuania;
using Core.Configuration;
using Core.Deductibles;
using Core.Dto;
using Core.TaxCalculators;

namespace Application.TaxCalculators.Default
{
    public class LTIncomeTaxCalculator : LTTaxCalculatorBase, ITaxCalculator
    {
        public override string TaxFieldName => nameof(Taxes.IncomeTax);
        public override string UniqueName => "LT:IncomeTax";

        public LTIncomeTaxCalculator
            (IConfigurationFactory configurationFactory, IDeductibleFactory deductibleFactory) :
            base(configurationFactory, deductibleFactory)
        {
        }
    }
}
