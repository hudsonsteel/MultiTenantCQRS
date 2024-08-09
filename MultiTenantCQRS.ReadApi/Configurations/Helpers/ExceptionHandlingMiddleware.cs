using Newtonsoft.Json;
using System.Net;

namespace MultiTenantCQRS.ReadApi.Configurations.Helpers
{
    public interface IExceptionResponseFormatter
    {
        string FormatExceptionResponse(HttpContext context, Exception exception);
    }

    public class JsonExceptionResponseFormatter : IExceptionResponseFormatter
    {
        public string FormatExceptionResponse(HttpContext context, Exception exception)
        {
            var response = new
            {
                StatusCode = context.Response.StatusCode,
                Message = "An error occurred while processing your request.",
                Detailed = exception.Message
            };

            return JsonConvert.SerializeObject(response);
        }
    }

    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;
        private readonly IExceptionResponseFormatter _responseFormatter;

        public ExceptionHandlingMiddleware(
            RequestDelegate next,
            ILogger<ExceptionHandlingMiddleware> logger,
            IExceptionResponseFormatter responseFormatter)
        {
            _next = next;
            _logger = logger;
            _responseFormatter = responseFormatter;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred.");
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var response = _responseFormatter.FormatExceptionResponse(context, exception);

            return context.Response.WriteAsync(response);
        }
    }

}
