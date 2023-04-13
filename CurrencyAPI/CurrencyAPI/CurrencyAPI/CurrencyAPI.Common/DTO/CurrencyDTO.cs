namespace CurrencyAPI.Common.DTO
{
    public class CurrencyDTO
    {
        public string? FromCurrency { get; set; } 
        public string? ToCurrency { get; set; }
        public string? Amout { get; set; } 
        public string Result { get; set; } = string.Empty;
    }
}
