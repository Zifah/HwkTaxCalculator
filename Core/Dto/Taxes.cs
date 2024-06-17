using System.Reflection;

namespace Core.Dto
{
    public record Taxes
    {
        public decimal GrossIncome { get; set; }
        public decimal IncomeTax { get; set; }
        public decimal SocialTax { get; set; }
        public decimal TotalTax { get; set; }
        public decimal Pension { get; set; }
        public decimal CharitySpent { get; set; }
        public decimal NetIncome => GrossIncome - TotalTax;


        public Taxes(IDictionary<string, decimal> taxes)
        {
            foreach (var kvp in taxes)
            {
                PropertyInfo? property = GetType().GetProperty(kvp.Key);
                if (property != null && property.CanWrite)
                {
                    property.SetValue(this, kvp.Value);
                }
                else
                {
                    throw new ArgumentException($"Unrecognized tax: '{kvp.Key}' could not be mapped.");
                }
            }

            TotalTax = taxes.Values.Sum();
        }
    }
}
