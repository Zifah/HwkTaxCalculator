using Core.Configuration;
using Core.Deductibles;
using Core.Dto;
using Core.TaxCalculators;

namespace Application.TaxCalculators
{
    public class DefaultSocialContributionCalculator : TaxCalculatorBase, ITaxCalculator
    {
        public override string TaxFieldName => nameof(Taxes.SocialTax);
        public override string UniqueName => "Default:SocialContribution";

        public DefaultSocialContributionCalculator
            (IConfigurationFactory configurationFactory, IDeductibleFactory deductibleFactory) :
            base(configurationFactory, deductibleFactory)
        { }

        public override bool IsApplicableTo(TaxPayer taxPayer)
        {
            // Here, one could apply rules like checking the taxpayer's country to see if this tax calculator applies to them.
            return true;
        }
    }
}
