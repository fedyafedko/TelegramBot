using Microsoft.AspNetCore.Mvc;
using CurrencyAPI.CurrencyBLL.Interfaces;
using Currency.BLL.Currency.API.Common.DTO;

namespace CurrencyAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CurrenciesController : ControllerBase
    {
        private readonly ICurrencyService _currencyService;

        public CurrenciesController(ICurrencyService currencyService)
        {
            _currencyService = currencyService;
        }
        [HttpGet("{have}")]
        public async Task<IActionResult> GetCurrencyByToHave(string have) => Ok(await _currencyService.GetCurrencyByToHave(have));

        [HttpPost]
        [ActionName(nameof(GetCurrencyByToHave))]
        public async Task<IActionResult> InsertCurrency(string have)
        {
            try
            {
                var result = await _currencyService.AddCurrency(have);
                return CreatedAtAction(nameof(_currencyService.GetCurrencyByToHave), new { have = result.old_amount }, result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("Calrulator")]
        public async Task<IActionResult> Calculator([FromQuery] string have, [FromQuery] string want, [FromQuery] int amount) => Ok(await _currencyService.CalculatorCurrency(have, want, amount));
       
        [HttpDelete("{have}")]
        public async Task<IActionResult> DeleteCurrency(string have) => await _currencyService.DeleteCurrency(have) ? Ok() : NotFound();
        [HttpGet]
        public IActionResult GetAll() => Ok(_currencyService.GetAll());

        [HttpPut("{have}")]
        public async Task<IActionResult> Update( string have, [FromBody]UpdateCurrencyDTO currency)
        {
            try
            {
                var result = await _currencyService.UpdateCurrency(currency, have);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

