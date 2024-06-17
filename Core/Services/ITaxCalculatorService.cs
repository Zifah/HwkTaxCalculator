using Core.Dto;

namespace Core.Services
{
    public interface ITaxCalculatorService
    {
        Taxes CalculateTaxes(TaxPayer taxPayer);
    }
}
