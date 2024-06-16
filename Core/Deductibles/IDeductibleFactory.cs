namespace Core.Deductibles
{
    public interface IDeductibleFactory
    {
        IDeductibleCalculator[] GetCalculators(string uniqueName);
    }
}