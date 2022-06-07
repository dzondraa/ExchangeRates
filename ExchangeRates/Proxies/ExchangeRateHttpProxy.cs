using ExchangeRates.Models;

namespace ExchangeRates.Proxies
{
    public class ExchangeRateHttpProxy : BaseProxy, IExchangeRateHttpProxy
	{
		private const string _clientName = "ExchangeApi";
		private readonly IHttpClientFactory _httpClientFactory;

		public ExchangeRateHttpProxy(IHttpClientFactory httpClientFactory)
		{
			ArgumentNullException.ThrowIfNull(httpClientFactory);
			_httpClientFactory = httpClientFactory;
		}

		public async Task<RateResponse> GetExchangeData(DateTime date, string from, string target)
		{
			var uri = $"/{date.ToString("yyyy-MM-dd")}{BuildQueryString(from, target)}";
			var response = await HandleTwinsApiMessage<RateResponse> (
				await _httpClientFactory.CreateClient(_clientName).GetAsync(uri));
			return response;
		}

		private string BuildQueryString(string from, string target)
		{
			Dictionary<string, string> queryParamPairs = new Dictionary<string, string>();

			if (!String.IsNullOrEmpty(from))
				queryParamPairs.Add("base", from);

			if (!String.IsNullOrEmpty(target))
				queryParamPairs.Add("symbols", target);

			return QueryString.Create(queryParamPairs).ToString();
		}

	}

	public abstract class BaseProxy
    {
		protected internal Task<T?> HandleTwinsApiMessage<T>(HttpResponseMessage message)
		{
			message.EnsureSuccessStatusCode();
			return message.Content.ReadFromJsonAsync<T>();
		}
	}

	public interface IExchangeRateHttpProxy
    {
		Task<RateResponse> GetExchangeData(DateTime date, string from, string target);
	}
}
