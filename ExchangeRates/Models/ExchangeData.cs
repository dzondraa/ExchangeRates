namespace ExchangeRates.Models
{
    public class ExchangeDataRequest
    {
        public IEnumerable<DateTime> Dates { get; set; }

        public string From { get; set; }

        public string Target { get; set; }

    }

    public class ExchangeDataResponse
    {
        public decimal Max { get; set; }
        
        public decimal Min { get; set; }

        public decimal Average { get; set; }
    }
}
