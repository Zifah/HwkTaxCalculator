using Core.Configuration;
using Core.Deductibles;
using Core.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.TaxCalculators.Default
{
    public abstract class DefaultTaxCalculatorBase : TaxCalculatorBase
    {
        protected DefaultTaxCalculatorBase(IConfigurationFactory configurationFactory, IDeductibleFactory deductibleFactory) : 
            base(configurationFactory, deductibleFactory)
        {
        }
        public override bool IsApplicableTo(TaxPayer taxPayer)
        {
            // Limiting the default to apply only to numeric SSNs
            return long.TryParse(taxPayer.SSN, out _);
        }
    }
}
