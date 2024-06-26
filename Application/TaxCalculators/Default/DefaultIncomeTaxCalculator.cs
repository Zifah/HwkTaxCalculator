﻿using Core.Configuration;
using Core.Deductibles;
using Core.Dto;
using Core.TaxCalculators;

namespace Application.TaxCalculators.Default
{
    public class DefaultIncomeTaxCalculator : DefaultTaxCalculatorBase, ITaxCalculator
    {
        public override string TaxFieldName => nameof(Taxes.IncomeTax);
        public override string UniqueName => "Default:IncomeTax";

        public DefaultIncomeTaxCalculator
            (IConfigurationFactory configurationFactory, IDeductibleFactory deductibleFactory) :
            base(configurationFactory, deductibleFactory)
        {
        }
    }
}
