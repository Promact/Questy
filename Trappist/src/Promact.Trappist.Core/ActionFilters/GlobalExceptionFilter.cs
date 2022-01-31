using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace Promact.Trappist.Core.ActionFilters
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<GlobalExceptionFilter> _logger;

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
            if (_logger.IsEnabled(LogLevel.Error))
                _logger.LogError(exception: context.Exception, message: context.ExceptionDispatchInfo.ToString());
        }
    }
}
