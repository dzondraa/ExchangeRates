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

        public HistoryService(IExchangeRateHttpProxy exchangeRateHttpProxy)
        {
            ArgumentNullException.ThrowIfNull(exchangeRateHttpProxy);
            _exchangeRateHttpProxy = exchangeRateHttpProxy;
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
                bag.Add(response);
            });
            await Task.WhenAll(tasks);
            return bag;
        }
    }
}
