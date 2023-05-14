using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CurrencyDAL.Entities;

public class CurrencyEntities
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string FromCurrency { get; set; } = string.Empty;
    public string ToCurrency { get; set; } = string.Empty;
    public double Amout { get; set; }
    public double Result { get; set; }
    public DateTime Data { get; set; } = DateTime.Now;
}
