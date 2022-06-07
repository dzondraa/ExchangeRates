using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRatesTests
{
    public class BaseTests
    {
        private readonly WebApplicationFactory<Program> _application = new WebApplicationFactory<Program>();
        private readonly RandomDateFactory _randomDateFactory = new RandomDateFactory();


        [Theory]
        [InlineData(10)]
        [InlineData(50)]
        [InlineData(100)]
        [InlineData(500)]
        public async Task Test10Dates(int numberOfDates)
        {
            var client = _application.CreateClient();
            
            var response = await client.GetAsync(BuildTestUrl(numberOfDates));

            response.EnsureSuccessStatusCode();
        }


        private string BuildTestUrl(int numberOfDates)
        {
            var dates = _randomDateFactory.GetNRandomDates(numberOfDates);
            var queryParamsList = new List<KeyValuePair<string, string>>();

            foreach (var date in dates)
            {
                queryParamsList.Add(new KeyValuePair<string, string>("Dates", date.ToString()));
            }
            queryParamsList.Add(new KeyValuePair<string, string>("from", "USD"));
            queryParamsList.Add(new KeyValuePair<string, string>("target", "EUR"));
            var finalUrl = QueryHelpers.AddQueryString("/api/history", queryParamsList);
            return finalUrl;
        }
    }
}
