using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyDAL.Entities;

public class CurrencyEntities
{
    [Key]
    public int Id { get; set; }
    public string FromCurrency { get; set; } = string.Empty;
    public string ToCurrency { get; set; } = string.Empty;
    public string Amout { get; set; } = string.Empty;
}
