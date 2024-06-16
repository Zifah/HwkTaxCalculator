using Core.Deductibles;

namespace Application.Deductibles
{
    public class DeductibleFactory : IDeductibleFactory
    {
        private readonly IEnumerable<IDeductibleCalculator> _deductibleCalculators;
        public DeductibleFactory(IEnumerable<IDeductibleCalculator> deductibleCalculators)
        {
            _deductibleCalculators = deductibleCalculators;
        }

        public IDeductibleCalculator[] GetCalculators(string taxName)
        {
            return _deductibleCalculators.Where(dc => dc.ApplicableTaxes.Contains(taxName)).ToArray();
        }
    }
}
