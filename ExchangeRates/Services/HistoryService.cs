using ExchangeRates.Exceptions;
using ExchangeRates.Models;
using ExchangeRates.Proxies;
using System.Collections.Concurrent;

namespace ExchangeRates.Services
{
    public interface IHistoryService
    {
        public Task<ExchangeDataResponse> Get(ExchangeDataRequest request);
    }

    public class HistoryService : IHistoryService
    {
        private readonly IExchangeRateHttpProxy _exchangeRateHttpProxy;
        private readonly ILogger<HistoryService> _logger;


        public HistoryService(IExchangeRateHttpProxy exchangeRateHttpProxy, ILogger<HistoryService> logger)
        {
            ArgumentNullException.ThrowIfNull(exchangeRateHttpProxy);
            ArgumentNullException.ThrowIfNull(logger);
            _exchangeRateHttpProxy = exchangeRateHttpProxy;
            _logger = logger;
        }

        public async Task<ExchangeDataResponse> Get(ExchangeDataRequest request)
        {
            var data = await GetDataConcurently(request);
            var valuesArray = data.Select(res => res.Rates[request.Target]).ToArray();
            return new ExchangeDataResponse
            {
                Max = valuesArray.Max(),
                Min = valuesArray.Min(),
                Average = valuesArray.Max()
            };
        }

        private async Task<ConcurrentBag<RateResponse>> GetDataConcurently(ExchangeDataRequest request)
        {
            var bag = new ConcurrentBag<RateResponse>();
            var tasks = request.Dates.Select(async date =>
            {
                    var response = await _exchangeRateHttpProxy.GetExchangeData(date, request.From, request.Target);
                if (response is not null)
                    bag.Add(response);
                else _logger.LogWarning($"Failed to retrieve data for {request}");
            });
            await Task.WhenAll(tasks);
            return bag.Count > 0 ? bag : throw new NoDataException("Historical data not found");
        }
    }
}
