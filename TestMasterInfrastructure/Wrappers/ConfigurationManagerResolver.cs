using System;
using System.Configuration;
using System.Threading.Tasks;
using Amazon;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Extensions.Caching;
using Newtonsoft.Json;

namespace TestMasterInfrastructure.Wrappers
{
    public static class ConfigurationManagerResolver
    {
        private const int OneHourMs = 10 * 60 * 1000;

        private static IAmazonSecretsManager _client = new AmazonSecretsManagerClient(
            new AmazonSecretsManagerConfig
            {
                RegionEndpoint = RegionEndpoint.GetBySystemName(ConfigurationManager.AppSettings["AWSRegion"])
            });

        private static SecretsManagerCache _cache = new SecretsManagerCache(
            new SecretCacheConfiguration()
            {
                Client = _client,
                CacheItemTTL = (uint)_сachedSecretsTTL
            });

        private static string _secretManagerPath = ConfigurationManager.AppSettings["AWSSecretsManagerPath"];

        private static int _сachedSecretsTTL
        {
            get
            {
                string cachedSecretsTtlStr = ConfigurationManager.AppSettings["CachedSecretsTimeToLiveMs"];
                int TTL;
                if (!int.TryParse(cachedSecretsTtlStr, out TTL))
                {
                    TTL = OneHourMs;
                }

                return TTL;
            }
        }

        public static ConnectionStringSettings GetConnectionString(string settingName)
        {
            var connectionString = GetSecretFromCache<ConnectionStringSettings>(settingName);

            if (!string.IsNullOrWhiteSpace(settingName) && string.IsNullOrWhiteSpace(connectionString.ConnectionString))
            {
                connectionString = ConfigurationManager.ConnectionStrings[settingName];
            }

            return connectionString;
        }

        public static T GetAppSection<T>(string sectionName) where T : new()
        {
            var section = GetSecretFromCache<T>(sectionName);

            if (!string.IsNullOrWhiteSpace(sectionName) && section.Equals(new T()))
            {
                section = (T)ConfigurationManager.GetSection(sectionName);
            }

            return section;
        }

        public static T GetAppSettings<T>(string settingName) where T : new()
        {
            var settingResult = GetSecretFromCache<T>(settingName);

            return settingResult;
        }

        public static string GetAppSettings(string settingName)
        {
            string settingResult = GetSecretFromCache(settingName);

            if (string.IsNullOrWhiteSpace(settingResult))
            {
                settingResult = ConfigurationManager.AppSettings[settingName];
            }

            return settingResult;
        }

        private static T GetSecretFromCache<T>(string secretPath) where T : new()
        {
            try
            {
                var config = ExtractParameter(secretPath);

                return JsonConvert.DeserializeObject<T>(config);
            }
            catch 
            {
                //Serialization or extract issue
            }
            return new T();
        }

        private static string GetSecretFromCache(string secretPath)
        {
            try
            {
                var config = ExtractParameter(secretPath);

                return config;
            }
            catch
            {
                return String.Empty;
            }

        }

        private static string ExtractParameter(string secretPath)
        {
            var result = Task.Run(() => _cache?.GetSecretString(_secretManagerPath)).Result;

            dynamic config = JsonConvert.DeserializeObject(result);

            if (!string.IsNullOrWhiteSpace(secretPath))
            {
                var pathToParam = secretPath.Split(':');

                foreach (var nextNote in pathToParam)
                {
                    config = config[nextNote];
                }
            }

            return config?.ToString();
        }

    }
}