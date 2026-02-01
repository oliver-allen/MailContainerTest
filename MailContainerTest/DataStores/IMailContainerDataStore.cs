using MailContainerTest.Types;

namespace MailContainerTest.DataStores
{
    public interface IMailContainerDataStore
    {
        public MailContainer GetMailContainer(string mailContainerNumber);

        public void UpdateMailContainer(MailContainer mailContainer);
    }
}
