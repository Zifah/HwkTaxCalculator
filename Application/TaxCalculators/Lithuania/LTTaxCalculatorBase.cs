using Core.Configuration;
using Core.Deductibles;
using Core.Dto;

namespace Application.TaxCalculators.Lithuania
{
    public abstract class LTTaxCalculatorBase : TaxCalculatorBase
    {
        public const string CountryCode = "LT";
        protected LTTaxCalculatorBase(IConfigurationFactory configurationFactory, IDeductibleFactory deductibleFactory) :
            base(configurationFactory, deductibleFactory)
        {

        }

        public override bool IsApplicableTo(TaxPayer taxPayer)
        {
            return taxPayer.SSN.StartsWith(CountryCode);
        }
    }
}
