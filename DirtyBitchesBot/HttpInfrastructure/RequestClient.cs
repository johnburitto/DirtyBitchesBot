using DirtyBitchesBot.Configuration;

namespace DirtyBitchesBot.HttpInfrastructure
{
    public class RequestClient
    {
        private HttpClient? _client;
        private static RequestClient? _instance;
        private ConfigurationService _configurationService = new ConfigurationService();

        public HttpClient Client => GetClient();
        public static RequestClient Instance => GetClientInstance();

        private HttpClient GetClient()
        {
            if (_client == null)
            {
                lock (new object())
                {
                    _client = new HttpClient
                    {
                        BaseAddress = new Uri(_configurationService.GetValue<string>("BaseURL") ?? "")
                    };
                }
            }

            return _client;
        }

        private static RequestClient GetClientInstance()
        {
            if (_instance == null)
            {
                lock (new object())
                {
                    _instance = new RequestClient();
                }
            }

            return _instance;
        }
    }
}
