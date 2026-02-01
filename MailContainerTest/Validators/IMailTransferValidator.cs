using MailContainerTest.Types;
using MailContainerTest.Types.DTOs;

namespace MailContainerTest.Validators
{
    public interface IMailTransferValidator
    {
        public bool IsValidForTransfer(MailContainer mailContainer, MakeMailTransferRequest request);
    }
}
