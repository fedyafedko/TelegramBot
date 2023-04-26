using Currency.BLL.Currency.API.Common.DTO;

namespace CurrencyAPI.CurrencyBLL.Interfaces
{
    public interface ICurrencyService
    {
        Task<CurrencyDTO> AddCurrency(string have);
        List<CurrencyDTO> GetAll();
        Task<CurrencyDTO> GetCurrencyByToHave(string have);
        Task<bool> DeleteCurrency(string have);
        Task<CurrencyDTO> UpdateCurrency(CurrencyDTO currency, string have);
        Task<string> CalculatorCurrency(string want, string have, int amount);

    }
}
