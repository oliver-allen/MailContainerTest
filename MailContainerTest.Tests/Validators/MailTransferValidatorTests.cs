using MailContainerTest.Types;
using MailContainerTest.Types.DTOs;
using MailContainerTest.Validators;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MailContainerTest.Tests.Validators
{
    [TestClass]
    public class MailTransferValidatorTests
    {
        private MailTransferValidator _validator;

        [TestInitialize]
        public void Setup()
        {
            _validator = new MailTransferValidator();
        }

        [TestMethod]
        public void IsValidForTransfer_WithValidContainer_ReturnsTrue()
        {
            // Arrange
            var mailContainer = CreateMailContainer(AllowedMailType.StandardLetter, 10, MailContainerStatus.Operational);
            var request = CreateTransferRequest(MailType.StandardLetter, 2);

            // Act
            var result = _validator.IsValidForTransfer(mailContainer, request);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsValidForTransfer_WithIncompatibleMailType_ReturnsFalse()
        {
            // Arrange
            var mailContainer = CreateMailContainer(AllowedMailType.LargeLetter, 10, MailContainerStatus.Operational);
            var request = CreateTransferRequest(MailType.StandardLetter, 2);

            // Act
            var result = _validator.IsValidForTransfer(mailContainer, request);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsValidForTransfer_WithInsufficientCapacity_ReturnsFalse()
        {
            // Arrange
            var mailContainer = CreateMailContainer(AllowedMailType.LargeLetter, 1, MailContainerStatus.Operational);
            var request = CreateTransferRequest(MailType.LargeLetter, 2);

            // Act
            var result = _validator.IsValidForTransfer(mailContainer, request);

            // Assert
            Assert.IsFalse(result);
        }

        private MailContainer CreateMailContainer(AllowedMailType allowedMailType, int capacity, MailContainerStatus status)
        {
            return new MailContainer
            {
                MailContainerNumber = "001",
                Capacity = capacity,
                Status = status,
                AllowedMailType = allowedMailType
            };
        }

        private MakeMailTransferRequest CreateTransferRequest(MailType mailType, int numberOfItems)
        {
            return new MakeMailTransferRequest
            {
                SourceMailContainerNumber = "SOURCE001",
                DestinationMailContainerNumber = "DEST001",
                NumberOfMailItems = numberOfItems,
                MailType = mailType,
                TransferDate = DateTime.Now
            };
        }
    }
}