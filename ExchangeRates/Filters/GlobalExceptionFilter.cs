using ExchangeRates.Exceptions;
using ExchangeRates.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Formatters;

namespace ExchangeRates.Filters
{
	public class GlobalExceptionFilter : IExceptionFilter
	{
		private readonly ILogger<GlobalExceptionFilter> _logger;

		public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger)
		{
			_logger = logger;
		}

		public virtual void OnException(ExceptionContext context)
		{
			var statusCode = StatusCodes.Status500InternalServerError;

			switch (context.Exception)
			{
				case ArgumentException:
					statusCode = StatusCodes.Status400BadRequest;
					break;

				case HttpRequestException:
					var httpRequestException = (HttpRequestException)context.Exception;
					if (httpRequestException.StatusCode.HasValue)
						statusCode = (int)httpRequestException.StatusCode;
					break;

				case NoDataException:
					statusCode = StatusCodes.Status404NotFound;
					break;

				default:
					_logger.LogError(context.Exception, "GlobalExceptionFilter caught an unknown exception.");
					break;
			}

			WriteErrorResponse(context, statusCode);
		}

		protected void WriteErrorResponse(ExceptionContext context, int statusCode)
		{
			var response = new ErrorResponse
			{
				StatusCode = statusCode,
				Message = context.Exception.Message,
				Data = context.Exception.Data
			};

			context.Result = new ObjectResult(response)
			{
				StatusCode = response.StatusCode,
				ContentTypes = new MediaTypeCollection { "application/problem+json" },
				DeclaredType = typeof(ErrorResponse)
			};
		}
	}
}
