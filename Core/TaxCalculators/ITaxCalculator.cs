using Core.Dto;

namespace Core.TaxCalculators
{
    public interface ITaxCalculator
    {
        string TaxFieldName { get; }
        string UniqueName { get; }

        /// <summary>
        /// Calculate this tax for a <paramref name="taxPayer"/>.
        /// </summary>
        /// <param name="taxPayer"></param>
        /// <returns></returns>
        decimal Calculate(TaxPayer taxPayer);

        /// <summary>
        /// Check to see if this tax applies to a <paramref name="taxPayer"/>.
        /// </summary>
        /// <param name="taxPayer"></param>
        /// <returns></returns>
        bool IsApplicableTo(TaxPayer taxPayer);
    }
}
