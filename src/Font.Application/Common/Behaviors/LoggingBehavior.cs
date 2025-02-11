using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Font.Application.Common.Behaviors
{
    public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull
    {
        private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

        public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var requestName = typeof(TRequest).Name;
            var uniqueId = Guid.NewGuid().ToString();

            _logger.LogInformation(
                "Beginning Request {RequestName} [{UniqueId}] at {StartTime}",
                requestName, uniqueId, DateTime.UtcNow);

            var timer = new Stopwatch();
            timer.Start();

            try
            {
                var response = await next();
                timer.Stop();

                _logger.LogInformation(
                    "Completed Request {RequestName} [{UniqueId}] in {ElapsedMilliseconds}ms",
                    requestName, uniqueId, timer.ElapsedMilliseconds);

                return response;
            }
            catch (Exception ex)
            {
                timer.Stop();
                _logger.LogError(
                    ex,
                    "Request {RequestName} [{UniqueId}] failed after {ElapsedMilliseconds}ms",
                    requestName, uniqueId, timer.ElapsedMilliseconds);
                throw;
            }
        }
    }
} 