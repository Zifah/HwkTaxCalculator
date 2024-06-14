namespace Core.Dto
{
    public record TaxPayer(string FullName, string SSN, DateTime DateOfBirth, decimal GrossIncome, decimal CharitySpent)
    {
    }
}
