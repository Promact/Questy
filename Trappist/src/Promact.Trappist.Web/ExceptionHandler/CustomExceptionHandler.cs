using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace Promact.Trappist.Web.ExceptionHandler
{
    public class CustomExceptionHandler : IExceptionFilter
    {
        private ILogger<CustomExceptionHandler> _logger;

        public CustomExceptionHandler(ILogger<CustomExceptionHandler> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Method fire when exception will thrown
        /// </summary>
        /// <param name="context">context object</param>
        public void OnException(ExceptionContext context)
        {
            _logger.LogError(context.Exception.ToString());
        }
    }
}