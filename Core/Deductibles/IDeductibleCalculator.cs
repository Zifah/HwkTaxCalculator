using Core.Dto;

namespace Core.Deductibles
{
    /// <summary>
    /// A deductible is an amount that must be deducted from the gross income before a tax is applied to the income.
    /// </summary>
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
