using Microsoft.Extensions.Logging;

namespace LoggerUnitTests
{
    public class PleaseTestMe
    {
        private readonly ILogger<PleaseTestMe> _logger;

        public PleaseTestMe(ILogger<PleaseTestMe> logger)
        {
            _logger = logger;
        }

        public void RunMe()
        {
            _logger.LogDebug("Logging this ...");
        }

        public void RunMeLoop()
        {
            for (int i = 0; i < 3; i++)
            {
                _logger.LogDebug("Logging Multiple Times ...");
            }
        }

        public void RunMeMultiDebug()
        {
            _logger.LogDebug("Message one.");
            _logger.LogDebug("Message two.");
            _logger.LogDebug("Message three.");
        }

        public void RunMeDifferentLevels()
        {
            _logger.LogDebug("Message one.");
            _logger.LogWarning("Message two.");
            _logger.LogError("Message three.");
        }
    }
}