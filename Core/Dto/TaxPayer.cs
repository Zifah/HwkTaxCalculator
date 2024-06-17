using System.ComponentModel.DataAnnotations;

namespace Core.Dto
{
    public record TaxPayer
    {
        [Required]
        public string FullName { get; set; }
        [Required]
        public string SSN { get; set; }
        [Required]
        public DateTime DateOfBirth { get; set; }
        [Required]
        public decimal GrossIncome { get; set; }
        public decimal CharitySpent { get; set; }
    }
}
