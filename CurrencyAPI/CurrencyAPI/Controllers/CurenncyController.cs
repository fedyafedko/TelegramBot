using Microsoft.AspNetCore.Mvc;
using CurrencyAPI.CurrencyBLL.Interfaces;
using CurrencyAPI.CurrencyBLL.Server;
using Currency.BLL.Currency.API.Common.DTO;

namespace CurrencyAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CurrencyController : ControllerBase
    {
        private readonly ICurrencyService _currencyService;
        public CurrencyController(ICurrencyService currencyService)
        {
            _currencyService = currencyService;
        }
        [HttpGet("{have}", Name = "GetCurrencyByToHave")]
        public async Task<CurrencyDTO> GetCurrencyByToHave(string have)
        {
            return await _currencyService.GetCurrencyByToHave(have);
        }
        [HttpPost(Name = "AddCurrencyController")]
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
        [HttpGet(Name = "Calrulator")]
        public async Task<IActionResult> Calculator(string have, string want, int amount)
        {
            return Ok(await _currencyService.CalculatorCurrency(have, want, amount));
        }
    }
}

