using Microsoft.AspNetCore.Mvc;
using CurrencyAPI.CurrencyBLL.Interfaces;
using CurrencyAPI.CurrencyBLL.Server;
using Currency.BLL.Currency.API.Common.DTO;
using CurrencyDAL.Entities;
using Newtonsoft.Json;
using System.Net.Http;
using AutoMapper;
using CurrencyAPI.Currency.DAL.Repositories.Interfaces;

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
        [HttpGet("GetCurrencyByToHave")]
        public async Task<IActionResult> GetCurrencyByToHave(string have) => Ok(await _currencyService.GetCurrencyByToHave(have));

        [HttpPost("AddCurrencyController")]
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
        public async Task<IActionResult> Calculator(string have, string want, int amount) => Ok(await _currencyService.CalculatorCurrency(have, want, amount));
       
        [HttpDelete("DeleteCurrency")]
        public async Task<IActionResult> DeleteCurrency(string have) => await _currencyService.DeleteCurrency(have) ? Ok() : NotFound();
        [HttpGet("GetAllCurrrency")]
        public IActionResult GetAll()
        {
            return Ok(_currencyService.GetAll());
        }
    }
}

