using MailContainerTest.Types.DTOs;

namespace MailContainerTest.Services
{
    public interface IMailTransferService
    {
        MakeMailTransferResult MakeMailTransfer(MakeMailTransferRequest request);
    }
}