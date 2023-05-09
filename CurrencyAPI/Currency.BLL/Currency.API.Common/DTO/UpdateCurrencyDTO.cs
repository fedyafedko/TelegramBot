using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Currency.BLL.Currency.API.Common.DTO
{
    public class UpdateCurrencyDTO
    {
        public string new_currency { get; set; }
        public double old_amount { get; set; }
        public double new_amount { get; set; }
        public DateTime date { get; set; }
    }
}
