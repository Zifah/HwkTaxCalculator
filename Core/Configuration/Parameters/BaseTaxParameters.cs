namespace Core.Configuration.Parameters
{
    public record BaseTaxParameters
    {
        public decimal Percentage { get; set; }
        public decimal TaxFreeAmount { get; set; }
        public decimal? MaximumTaxableAmount { get; set; }
    }
}
