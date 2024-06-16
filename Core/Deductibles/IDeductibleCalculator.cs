using Core.Dto;

namespace Core.Deductibles
{
    public interface IDeductibleCalculator
    {
        /// <summary>
        /// A collection of taxes to which this deductible applies.
        /// </summary>
        string[] ApplicableTaxes { get; }
        string UniqueName { get; }
        decimal Calculate(TaxPayer taxPayer);
    }
}
