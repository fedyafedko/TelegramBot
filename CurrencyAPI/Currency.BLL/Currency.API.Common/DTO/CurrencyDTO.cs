using Newtonsoft.Json;

namespace Currency.BLL.Currency.API.Common.DTO
{
    public class CurrencyDTO
    {
        
        public string old_currency { get; set; }
        public string new_currency { get; set; }
        public double old_amount { get; set; }
        public double new_amount { get; set; }
    }

}
