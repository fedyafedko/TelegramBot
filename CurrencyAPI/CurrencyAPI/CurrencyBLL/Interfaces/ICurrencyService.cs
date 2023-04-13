using CurrencyAPI.Common.DTO;
namespace CurrencyAPI.CurrencyBLL.Interfaces
{
    public interface ICurrencyService
    {
        Task<CurrencyDTO> AddCurrency(CurrencyDTO currency); 
        List<CurrencyDTO> GetAll();
        Task<CurrencyDTO> GetCurrencyById(int id);
        Task<bool> DeleteCurrency(int id);
        Task<CurrencyDTO> UpdateCurrency(CurrencyDTO currency);
    }
}
