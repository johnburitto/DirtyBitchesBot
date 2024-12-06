using Microsoft.Extensions.Configuration;

namespace DirtyBitchesBot.Configuration
{
    public class ConfigurationService
    {
        private IConfiguration _configuration;

        public ConfigurationService()
        {
            _configuration = GetConfiguration();
        }

        private IConfiguration GetConfiguration()
        {
            if (_configuration == null)
            {
                lock (new object())
                {
                    _configuration = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json", optional: false)
                        .Build();
                }
            }

            return _configuration;
        }

        public T? GetValue<T>(string section)
        {
            return _configuration.GetValue<T>(section);
        }
    }
}
