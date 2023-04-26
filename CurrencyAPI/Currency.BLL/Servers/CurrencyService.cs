using AutoMapper;
using CurrencyAPI.CurrencyBLL.Interfaces;
using CurrencyAPI.Currency.DAL.Repositories.Interfaces;
using CurrencyDAL.Entities;
using Newtonsoft.Json;
using CurrencyDAL.EF;
using System;
using Currency.BLL.Currency.API.Common.DTO;
using Newtonsoft.Json.Serialization;

namespace CurrencyAPI.CurrencyBLL.Server
{
    public class CurrencyService : ICurrencyService
    {
        private readonly ICurrencyRepository _currencyRepository;
        private readonly IMapper _mapper;
        private static readonly HttpClient _httpClient = new HttpClient();
        private static readonly string baseUri = "https://api.api-ninjas.com/v1/convertcurrency";

        public CurrencyService(ICurrencyRepository repository, IMapper mapper)
        {
            _currencyRepository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

        }
        public async Task<CurrencyDTO> AddCurrency(string old_currency)
        {
            var uri = $"{baseUri}?have={old_currency}&want=UAH&amount=1";
            var response = await _httpClient.GetAsync(uri);
            response.EnsureSuccessStatusCode();
            var jsonString = await response.Content.ReadAsStringAsync();
            var currencyDTO = JsonConvert.DeserializeObject<CurrencyDTO>(jsonString);
            CurrencyEntities entity = _mapper.Map<CurrencyEntities>(currencyDTO);

            await _currencyRepository.AddAsync(entity);

            return _mapper.Map<CurrencyDTO>(entity);
        }
        public async Task<bool> DeleteCurrency(string have)
        {
            var currency = await _currencyRepository.FindAsync(have);
            return currency != null && await _currencyRepository.DeleteAsync(currency) > 0;
        }

        public Task<List<CurrencyDTO>> GetAll()
        {
            return Task.FromResult(_mapper.Map<IEnumerable<CurrencyDTO>>(_currencyRepository.GetAll()).ToList());
        }

        public async Task<CurrencyDTO> GetCurrencyByToHave(string have)
        {
            CurrencyEntities? currency = await _currencyRepository.FindAsync(have);
            return currency != null ? _mapper.Map<CurrencyDTO>(currency) : null!;
        }

        public async Task<CurrencyDTO> UpdateCurrency(CurrencyDTO currency, string have)
        {
            var entity = await _currencyRepository.FindAsync(have);

            _mapper.Map(currency, entity);

            await _currencyRepository.UpdateAsync(entity);

            return _mapper.Map<CurrencyDTO>(entity);
        }

        public async Task<string> CalculatorCurrency(string have, string want, int amount)
        {
            var uri = $"{baseUri}?have={have}&want={want}&amount={amount}";
            var response = await _httpClient.GetAsync(uri);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return result;
        }
    }
}
