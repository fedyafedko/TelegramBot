﻿using AutoMapper;
using CurrencyAPI.CurrencyBLL.Interfaces;
using CurrencyAPI.Currency.DAL.Repositories.Interfaces;
using CurrencyDAL.Entities;
using Newtonsoft.Json;
using Currency.BLL.Currency.API.Common.DTO;


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
            if (await _currencyRepository.Table.FindAsync(old_currency) != null)
                throw new InvalidOperationException("Entity with such key already exists in database");

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
            var entity = await _currencyRepository.FindAsync(have);
            if (entity == null)
                throw new KeyNotFoundException($"Unable to find entity with such key {have}");
            var currency = await _currencyRepository.FindAsync(have);
            return currency != null && await _currencyRepository.DeleteAsync(currency) > 0;
        }

        public List<CurrencyDTO> GetAll()
        {
            var currencies = _currencyRepository.GetAll();
            if (currencies == null || !currencies.Any())
                throw new InvalidOperationException("No currencies found");
            return _mapper.Map<IEnumerable<CurrencyDTO>>(currencies).ToList();
        }

        public async Task<CurrencyDTO> GetCurrencyByToHave(string have)
        {
            var entity = await _currencyRepository.FindAsync(have);
            if (entity == null)
            {
                throw new KeyNotFoundException($"Unable to find entity with such key {have}");
            }
            CurrencyEntities? currency = await _currencyRepository.FindAsync(have);
            return currency != null ? _mapper.Map<CurrencyDTO>(currency) : null!;
        }

        public async Task<UpdateCurrencyDTO> UpdateCurrency(UpdateCurrencyDTO currency, string have)
        {
            var entity = await _currencyRepository.FindAsync(have);
            if (entity == null)
            {
                throw new KeyNotFoundException($"Unable to find entity with such key {have}");
            }
            _mapper.Map(currency, entity);

            await _currencyRepository.UpdateAsync(entity);

            return _mapper.Map<UpdateCurrencyDTO>(entity);
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
