using MailContainerTest.DataStores;
using MailContainerTest.Factories;
using MailContainerTest.Services;
using MailContainerTest.Types;
using MailContainerTest.Types.DTOs;
using MailContainerTest.Validators;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace MailContainerTest.Tests.Services
{
    [TestClass]
    public class MailTransferServiceTests
    {
        private Mock<IMailContainerDataStoreFactory> _mailContainerDataStoreFactoryMock;
        private Mock<IMailContainerDataStore> _mailContainerDataStoreMock;
        private Mock<IMailTransferValidator> _mailTransferValidatorMock;
        private MailTransferService _service;

        [TestInitialize]
        public void Setup()
        {
            _mailContainerDataStoreFactoryMock = new Mock<IMailContainerDataStoreFactory>();
            _mailContainerDataStoreMock = new Mock<IMailContainerDataStore>();
            _mailTransferValidatorMock = new Mock<IMailTransferValidator>();

            _mailContainerDataStoreFactoryMock
                .Setup(factory => factory.CreateDataStore())
                .Returns(_mailContainerDataStoreMock.Object);

            _service = new MailTransferService(
                _mailContainerDataStoreFactoryMock.Object,
                _mailTransferValidatorMock.Object);
        }

        [TestMethod]
        public void MakeMailTransfer_WithValidTransfer_ReturnsSuccessResult()
        {
            // Arrange
            var sourceMailContainerNumber = "SOURCE001";
            var request =  CreateTransferRequest(sourceMailContainerNumber, 2);
            var mailContainer = CreateMailContainer(10);

            _mailContainerDataStoreMock
                .Setup(dataStore => dataStore.GetMailContainer(sourceMailContainerNumber))
                .Returns(mailContainer);
            _mailTransferValidatorMock
                .Setup(validator => validator.IsValidForTransfer(mailContainer, request))
                .Returns(true);

            // Act
            var result = _service.MakeMailTransfer(request);

            // Assert
            Assert.IsTrue(result.Success);
            Assert.AreEqual(8, mailContainer.Capacity); // 10 - 2
            _mailContainerDataStoreMock.Verify(dataStore => dataStore.UpdateMailContainer(mailContainer), Times.Once);
        }

        [TestMethod]
        public void MakeMailTransfer_WithInvalidTransfer_ReturnsFailureResult()
        {
            // Arrange
            var sourceMailContainerNumber = "SOURCE001";
            var request =  CreateTransferRequest(sourceMailContainerNumber, 2);
            var mailContainer = CreateMailContainer(10);

            _mailContainerDataStoreMock
                .Setup(dataStore => dataStore.GetMailContainer(sourceMailContainerNumber))
                .Returns(mailContainer);
            _mailTransferValidatorMock
                .Setup(validator => validator.IsValidForTransfer(mailContainer, request))
                .Returns(false);

            // Act
            var result = _service.MakeMailTransfer(request);

            // Assert
            Assert.IsFalse(result.Success);
            Assert.AreEqual(10, mailContainer.Capacity);
            _mailContainerDataStoreMock.Verify(dataStore => dataStore.UpdateMailContainer(mailContainer), Times.Never);
        }

        private MakeMailTransferRequest CreateTransferRequest(string sourceMailContainerNumber, int numberOfMailItems)
        {
            return new MakeMailTransferRequest
            {
                SourceMailContainerNumber = sourceMailContainerNumber,
                DestinationMailContainerNumber = "DEST001",
                NumberOfMailItems = numberOfMailItems,
                MailType = MailType.StandardLetter,
                TransferDate = DateTime.Now
            };
        }

        private MailContainer CreateMailContainer(int capacity)
        {
            return new MailContainer
            {
                MailContainerNumber = "001",
                Capacity = capacity,
                Status = MailContainerStatus.Operational,
                AllowedMailType = AllowedMailType.StandardLetter
            };
        }
    }
}
