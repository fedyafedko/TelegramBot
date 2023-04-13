using System.Reflection;
using AutoMapper;
using CurrencyAPI.Common.DTO;
using CurrencyAPI.CurrencyBLL.Interfaces;
using CurrencyAPI.CurrencyDAL.Repositories.Interfaces;

namespace CurrencyAPI.CurrencyBLL.Server
{
    public class CurrencyServer : ICurrencyService
    {
        private readonly ICurrencyRepository _currencyRepository;
        private readonly IMapper _mapper; 
        public Task<CurrencyDTO> AddCurrency(CurrencyDTO currency)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteCurrency(int id)
        {
            throw new NotImplementedException();
        }

        public List<CurrencyDTO> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<CurrencyDTO> GetCurrencyById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<CurrencyDTO> UpdateCurrency(CurrencyDTO currency)
        {
            throw new NotImplementedException();
        }
    }
}
