using MailContainerTest.Types.Configuration;

namespace MailContainerTest.Configuration
{
    public interface IConfigurationService
    {
        public DataStoreType GetDataStoreType();
    }
}
