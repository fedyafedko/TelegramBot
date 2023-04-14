using CurrencyDAL.Entities;
using CurrencyDAL.Repositories;

namespace CurrencyAPI.Currency.DAL.Repositories.Interfaces
{
    public interface ICurrencyRepository : IRepo<CurrencyEntities, string>
    {

    }
}
