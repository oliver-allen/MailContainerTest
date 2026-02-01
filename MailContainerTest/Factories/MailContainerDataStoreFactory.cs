using MailContainerTest.Configuration;
using MailContainerTest.DataStores;
using MailContainerTest.Types.Configuration;

namespace MailContainerTest.Factories
{
    public class MailContainerDataStoreFactory : IMailContainerDataStoreFactory
    {
        private readonly IConfigurationService _configurationService;

        public MailContainerDataStoreFactory(IConfigurationService configService) 
        {
            _configurationService = configService;
        }

        public IMailContainerDataStore CreateDataStore()
        {
            var dataStoreType = _configurationService.GetDataStoreType();

            // NOTE: This would likely use the host's dependency injection container to resolve
            // the concrete implementation and any dependencies it may have.
            return dataStoreType switch
            {
                DataStoreType.Primary => new MailContainerDataStore(),
                DataStoreType.Backup => new BackupMailContainerDataStore(),
                _ => throw new NotSupportedException($"DataStoreType '{dataStoreType}' is not supported.")
            };
        }
    }
}
