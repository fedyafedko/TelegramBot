
using CurrencyDAL.Entities;
using CurrencyDAL.Repositories;
using CurrencyAPI.Currency.DAL.Repositories.Interfaces;
using CurrencyDAL.EF;

namespace CurrencyAPI.DAL.Repositories;
public class CurrencyRepository : Repo<CurrencyEntities, string>, ICurrencyRepository
{
    public CurrencyRepository(ApplicationDbContext context) : base(context) { }
}
