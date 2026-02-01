using System.Configuration;
using MailContainerTest.Types.Configuration;

namespace MailContainerTest.Configuration
{
    public class AppSettingsConfigurationService : IConfigurationService
    {
        public DataStoreType GetDataStoreType()
        {
            var dataStoreTypeValue = ConfigurationManager.AppSettings["DataStoreType"];

            // NOTE: To keep logic consistant to before the refactor,
            // not falling back to "Backup" if AppSettings value is invalid.
            return Enum.TryParse(dataStoreTypeValue, out DataStoreType result)
                ? result
                : DataStoreType.Primary;
        }
    }
}
