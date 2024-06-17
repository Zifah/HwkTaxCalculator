using Core.Configuration;
using Core.Deductibles;
using Core.Dto;
using Core.TaxCalculators;

namespace Application.TaxCalculators.Default
{
    public class DefaultIncomeTaxCalculator : TaxCalculatorBase, ITaxCalculator
    {
        public override string TaxFieldName => nameof(Taxes.IncomeTax);
        public override string UniqueName => "Default:IncomeTax";

        public DefaultIncomeTaxCalculator
            (IConfigurationFactory configurationFactory, IDeductibleFactory deductibleFactory) :
            base(configurationFactory, deductibleFactory)
        {
        }

        public override bool IsApplicableTo(TaxPayer taxPayer)
        {
            // Here, one could apply rules like checking the taxpayer's country to see if this tax calculator applies to them.
            return true;
        }
    }
}
