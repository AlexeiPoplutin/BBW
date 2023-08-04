using System;
using BBW.Plugins.Opportunity.Interfaces;
using BBW.Plugins.Opportunity.Models;
using BBW.Plugins.Opportunity.Services;
using Microsoft.Xrm.Sdk;
using Moq;
using NUnit.Framework;

namespace BBW.Plugins.Tests.Services
{
    [TestFixture]
    public class SetStatusServiceTests
    {
        private SetStatusService _setStatusService;
        private Mock<ITracingService> _tracingServiceMock;
        private Mock<IStatusCalculator> _statusCalculatorMock;

        [SetUp]
        public void SetUp()
        {
            _tracingServiceMock = new Mock<ITracingService>();
            _statusCalculatorMock = new Mock<IStatusCalculator>();
            _setStatusService = new SetStatusService(_tracingServiceMock.Object, _statusCalculatorMock.Object);
        }

        [Test]
        public void SetStatus_Should_NotCall_If_ShouldServiceBeExecuted_Returns_False()
        {
            // Arrange
            var opportunity = new Entity("opportunity") { Id = Guid.NewGuid() };
            var preImage = new Entity("opportunity") { Id = Guid.NewGuid() };

            // Act
            _setStatusService.SetStatus(opportunity, preImage);

            // Assert
            _statusCalculatorMock.Verify(mock => mock.CalculateBasedOnRevenueAndCloseProbability(It.IsAny<Entity>()), Times.Never);
        }

        [TestCase((int)OpportunityEntity.Status_OptionSet.Open, (int)OpportunityEntity.StatusReason_OptionSet.InProgress)]
        [TestCase((int)OpportunityEntity.Status_OptionSet.Open, null)]
        [TestCase(null, (int)OpportunityEntity.StatusReason_OptionSet.InProgress)]
        public void SetStatus_Should_NotUpdateStateAndStatus_If_IsStatusUpdateNeeded_Returns_False(int? calculatedState, int? calculatedStatus)
        {
            // Arrange
            var opportunity = new Entity("opportunity") { Id = Guid.NewGuid() };
            opportunity["closeprobability"] = new Random().Next(0, 100);

            var preImage = new Entity("opportunity") { Id = opportunity.Id };
            preImage["statecode"] = new OptionSetValue((int)OpportunityEntity.Status_OptionSet.Open);
            preImage["statuscode"] = new OptionSetValue((int)OpportunityEntity.StatusReason_OptionSet.InProgress);

            var oppStatusTuple = makeTuple(
                calculatedState.HasValue ? new OptionSetValue(calculatedState.Value) : null,
                calculatedStatus.HasValue ? new OptionSetValue(calculatedStatus.Value) : null);

            _statusCalculatorMock.Setup(mock => mock.CalculateBasedOnRevenueAndCloseProbability(It.IsAny<Entity>()))
                .Returns(oppStatusTuple);

            // Act
            _setStatusService.SetStatus(opportunity, preImage);

            // Assert
            Assert.IsTrue(!opportunity.Contains("statecode"));
            Assert.IsTrue(!opportunity.Contains("statuscode"));
        }

        [Test]
        public void SetStatus_Should_UpdateStatus_If_IsStatusUpdateNeeded_Returns_True()
        {
            // Arrange
            var opportunity = new Entity("opportunity") { Id = Guid.NewGuid() };
            opportunity["closeprobability"] = new Random().Next(0, 100);

            var preImage = new Entity("opportunity") { Id = opportunity.Id };
            preImage["statecode"] = new OptionSetValue((int)OpportunityEntity.Status_OptionSet.Open);
            preImage["statuscode"] = new OptionSetValue((int)OpportunityEntity.StatusReason_OptionSet.InProgress);

            var oppStatusTuple = makeTuple(
                new OptionSetValue((int)OpportunityEntity.Status_OptionSet.Won),
                new OptionSetValue((int)OpportunityEntity.StatusReason_OptionSet.Won));

            _statusCalculatorMock.Setup(mock => mock.CalculateBasedOnRevenueAndCloseProbability(It.IsAny<Entity>()))
                .Returns(oppStatusTuple);

            // Act
            _setStatusService.SetStatus(opportunity, preImage);

            // Assert
            Assert.IsTrue(opportunity.Contains("statecode"));
            Assert.IsTrue(opportunity.Contains("statuscode"));
            Assert.AreEqual((int)OpportunityEntity.Status_OptionSet.Won, opportunity.GetAttributeValue<OptionSetValue>("statecode").Value);
            Assert.AreEqual((int)OpportunityEntity.StatusReason_OptionSet.Won, opportunity.GetAttributeValue<OptionSetValue>("statuscode").Value);
        }

        private (OptionSetValue oppState, OptionSetValue oppStatus) makeTuple(OptionSetValue oppStateIn, OptionSetValue oppStatusIn)
        {
            return (oppStateIn, oppStatusIn);
        }
    }
}
