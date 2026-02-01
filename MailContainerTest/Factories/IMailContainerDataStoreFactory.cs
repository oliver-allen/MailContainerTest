using MailContainerTest.DataStores;

namespace MailContainerTest.Factories
{
    public interface IMailContainerDataStoreFactory
    {
        public IMailContainerDataStore CreateDataStore();
    }
}
