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
        public async Task<IActionResult> GetCurrencyByToHave(string have)
        {
            try
            {
                return Ok(await _currencyService.GetCurrencyByToHave(have));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
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
        public async Task<IActionResult> Calculator([FromQuery] string have, [FromQuery] string want, [FromQuery] int amount)
        {
            try
            {
                return Ok(await _currencyService.CalculatorCurrency(have, want, amount));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{have}")]
        public async Task<IActionResult> DeleteCurrency(string have)
        {
            try
            {

                return await _currencyService.DeleteCurrency(have) ? Ok() : NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                return Ok(_currencyService.GetAll());
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{have}")]
        public async Task<IActionResult> Update(string have, [FromBody] UpdateCurrencyDTO currency)
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

