﻿using Core.Configuration;
using Core.Deductibles;
using Core.Dto;
using Core.TaxCalculators;

namespace Application.TaxCalculators.Default
{
    public class DefaultSocialContributionCalculator : DefaultTaxCalculatorBase, ITaxCalculator
    {
        public override string TaxFieldName => nameof(Taxes.SocialTax);
        public override string UniqueName => "Default:SocialContribution";

        public DefaultSocialContributionCalculator
            (IConfigurationFactory configurationFactory, IDeductibleFactory deductibleFactory) :
            base(configurationFactory, deductibleFactory)
        { }
    }
}
