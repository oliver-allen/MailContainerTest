using MailContainerTest.Factories;
using MailContainerTest.Types.DTOs;
using MailContainerTest.Validators;

namespace MailContainerTest.Services
{
    public class MailTransferService : IMailTransferService
    {
        private readonly IMailContainerDataStoreFactory _mailContainerDataStoreFactory;
        private readonly IMailTransferValidator _mailTransferValidator;

        public MailTransferService(IMailContainerDataStoreFactory mailContainerDataStoreFactory, IMailTransferValidator mailTransferValidator)
        {
            _mailContainerDataStoreFactory = mailContainerDataStoreFactory;
            _mailTransferValidator = mailTransferValidator;
        }

        public MakeMailTransferResult MakeMailTransfer(MakeMailTransferRequest request)
        {
            // NOTE: This factory pattern should likely be moved into the infrustrcutre
            // layer and an IMailContainerDataStore would be injected into this class instead.
            var mailContainerDataStore = _mailContainerDataStoreFactory.CreateDataStore();
            var mailContainer = mailContainerDataStore.GetMailContainer(request.SourceMailContainerNumber);
            
            var isValidForTransfer = _mailTransferValidator.IsValidForTransfer(mailContainer, request);
            if (isValidForTransfer)
            {
                mailContainer.Capacity -= request.NumberOfMailItems;
                mailContainerDataStore.UpdateMailContainer(mailContainer);
            }

            // NOTE: Not dealing with the destination mail container as it wasn't in the original code.
            return new MakeMailTransferResult()
            {
                Success = isValidForTransfer
            };
        }
    }
}
