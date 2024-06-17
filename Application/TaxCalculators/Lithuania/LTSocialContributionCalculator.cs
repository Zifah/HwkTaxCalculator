using Application.TaxCalculators.Lithuania;
using Core.Configuration;
using Core.Deductibles;
using Core.Dto;
using Core.TaxCalculators;

namespace Application.TaxCalculators.Default
{
    public class LTSocialContributionCalculator : LTTaxCalculatorBase, ITaxCalculator
    {
        public override string TaxFieldName => nameof(Taxes.SocialTax);
        public override string UniqueName => "LT:SocialContribution";

        public LTSocialContributionCalculator
            (IConfigurationFactory configurationFactory, IDeductibleFactory deductibleFactory) :
            base(configurationFactory, deductibleFactory)
        {
        }
    }
}
