# ExchangeRates
High load API for getting historical exchange data

## How to run
- clone the repo
- dotnet run
## Tests:
Added some examples for testing calls with:
- 50 dates
- 100 dates
- 500 dates
- 1000 dates

It would be perfect if you can run it from VisualStudio test explorer for better view of time spent etc.
For ref: https://github.com/dzondraa/ExchangeRates/blob/master/ExchangeRatesTests/BaseTests.cs
## Potential issues
- Managing HTTP Clients
- Choosing the right way of collecting responses concurently

## Approaches
- Used IHttpClientFactory for safe handle and better resource management
- Preventing _Socket exhaustion problem_
- Collect responses concurently in thread-safe collection (example: ConcurentBag<T>)
