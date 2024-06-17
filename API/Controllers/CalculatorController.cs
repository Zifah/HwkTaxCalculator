using Core.Dto;
using Core.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CalculatorController : ControllerBase
    {
        private readonly ITaxCalculatorService _taxCalculatorService;

        public CalculatorController(ITaxCalculatorService taxCalculatorService)
        {
            _taxCalculatorService = taxCalculatorService;

        }

        [HttpPost]
        public IActionResult Post([FromBody] TaxPayer taxPayerData)
        {
            var taxes = _taxCalculatorService.CalculateTaxes(taxPayerData);
            return Ok(taxes);
        }
    }
}
