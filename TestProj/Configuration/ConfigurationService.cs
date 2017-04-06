using System.Configuration;

namespace TestProj.Configuration
{
    public class ConfigurationService : IConfigurationService
    {
        public string GetConfigurationValue(string configurationDesignator)
        {
            var configurationValue = ConfigurationManager.AppSettings[configurationDesignator];
            return configurationValue;
        }
    }
}
