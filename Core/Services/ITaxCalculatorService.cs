using Core.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services
{
    public interface ITaxCalculatorService
    {
        Taxes CalculateTaxes(TaxPayer taxPayer);
    }
}
