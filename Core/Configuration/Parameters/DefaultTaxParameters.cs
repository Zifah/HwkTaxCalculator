using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Configuration.Parameters
{
    public record DefaultTaxParameters
    {
        public decimal Percentage { get; set; }
        public decimal TaxFreeAmount { get; set; }
        public decimal? MaximumTaxableAmount { get; set; }
    }
}
