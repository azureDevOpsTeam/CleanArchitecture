using Microsoft.Extensions.Logging;

namespace ApplicationLayer.Extensions
{
    public static class LoggerExtensions
    {
        public static void Log(this ILogger logger, LogLevel logLevel, Exception exception)
        {
            if (exception == null)
            {
                return;
            }

            var message = exception.InnerException is not null
                ? $"Inner Exception: {exception.InnerException.Message}"
                : exception.Message;

            switch (logLevel)
            {
                case LogLevel.Error:
                    logger.LogError(exception.Message, message);
                    break;

                case LogLevel.Warning:
                    logger.LogWarning(exception.Message, message);
                    break;

                case LogLevel.Information:
                    logger.LogInformation(exception.Message, message);
                    break;

                case LogLevel.Debug:
                    logger.LogDebug(exception.Message, message);
                    break;

                case LogLevel.Critical:
                    logger.LogCritical(exception.Message, message);
                    break;

                default:
                    logger.LogTrace(exception.Message, message);
                    break;
            }
        }
    }
}