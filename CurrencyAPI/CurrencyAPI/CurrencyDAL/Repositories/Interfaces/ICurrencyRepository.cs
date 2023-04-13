using CurrencyDAL.Entities;
using CurrencyDAL.Repositories;

namespace CurrencyAPI.CurrencyDAL.Repositories.Interfaces
{
    public interface ICurrencyRepository : IRepo<CurrencyEntities, string>
    {
    }
}
