using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace Promact.Trappist.Core.ActionFilters
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        private ILogger<GlobalExceptionFilter> _logger;

        public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger)
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
