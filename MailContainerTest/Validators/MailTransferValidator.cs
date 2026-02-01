using MailContainerTest.Types;
using MailContainerTest.Types.DTOs;

namespace MailContainerTest.Validators
{
    // NOTE: This could be a static class if it doesn't require additional dependencies in the future.
    public class MailTransferValidator : IMailTransferValidator
    {
        public bool IsValidForTransfer(MailContainer mailContainer, MakeMailTransferRequest request)
        {
            if (mailContainer == null)
            {
                return false;
            }

            var requestMailType = request.MailType;

            if (!IsValidMailType(mailContainer, requestMailType))
            {
                return false;
            }

            return requestMailType switch
            {
               MailType.LargeLetter => IsValidForLargeLetter(mailContainer, request),
               MailType.SmallParcel => IsValidForSmallParcel(mailContainer),
               _ => true
            };
        }

        private bool IsValidMailType(MailContainer mailContainer, MailType mailType)
        {
            // NOTE: Could create mapping logic between enum types and move somewhere else
            return mailType switch
            {
                MailType.StandardLetter => mailContainer.AllowedMailType.HasFlag(AllowedMailType.StandardLetter),
                MailType.LargeLetter => mailContainer.AllowedMailType.HasFlag(AllowedMailType.LargeLetter),
                MailType.SmallParcel => mailContainer.AllowedMailType.HasFlag(AllowedMailType.SmallParcel),
                _ => false
            };
        }

        private bool IsValidForLargeLetter(MailContainer mailContainer, MakeMailTransferRequest request)
        {
            return mailContainer.Capacity >= request.NumberOfMailItems;
        }

        private bool IsValidForSmallParcel(MailContainer mailContainer)
        {
            return mailContainer.Status == MailContainerStatus.Operational;
        }
    }
}
