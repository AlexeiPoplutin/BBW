using System;
using BBW.Plugins.Opportunity;
using BBW.Plugins.Opportunity.Models;
using BBW.Plugins.Opportunity.Services;
using Microsoft.Xrm.Sdk;
using NUnit.Framework;

namespace BBW.Plugins.Tests.Services
{
    [TestFixture()]
    public class StatusCalculatorTests
    {
        private Entity opportunity;

        [SetUp]
        public void SetUp()
        {
            opportunity = new Entity("opportunity", Guid.NewGuid());
        }


        [TestCase(0, 50, (int)OpportunityEntity.Status_OptionSet.Lost, (int)OpportunityEntity.StatusReason_OptionSet.Canceled)]
        [TestCase(100, 0, (int)OpportunityEntity.Status_OptionSet.Lost, (int)OpportunityEntity.StatusReason_OptionSet.Canceled)]
        [TestCase(0, 0, (int)OpportunityEntity.Status_OptionSet.Lost, (int)OpportunityEntity.StatusReason_OptionSet.Canceled)]

        [TestCase(Constants.CloseProbabilityToWin, Constants.AmountToWin, (int)OpportunityEntity.Status_OptionSet.Won, (int)OpportunityEntity.StatusReason_OptionSet.Won)]

        [TestCase(Constants.CloseProbabilityToWin, Constants.AmountToWin-1, (int)OpportunityEntity.Status_OptionSet.Open, (int)OpportunityEntity.StatusReason_OptionSet.InProgress)]
        [TestCase(Constants.CloseProbabilityToWin-1, Constants.AmountToWin, (int)OpportunityEntity.Status_OptionSet.Open, (int)OpportunityEntity.StatusReason_OptionSet.InProgress)]
        [TestCase(Constants.CloseProbabilityToWin, null, (int)OpportunityEntity.Status_OptionSet.Open, (int)OpportunityEntity.StatusReason_OptionSet.InProgress)]
        [TestCase(null, Constants.AmountToWin, (int)OpportunityEntity.Status_OptionSet.Open, (int)OpportunityEntity.StatusReason_OptionSet.InProgress)]

        public void CalculateBasedOnRevenueAndCloseProbability_CalculatesExpectedOutput(int? closeProb, int? estimatedValue, int expectedState, int expectedStatus)
        {
            // Arrange
            opportunity["closeprobability"] = closeProb;
            opportunity["estimatedvalue"] = estimatedValue.HasValue ? new Money(estimatedValue.Value) : null;

            // Act
            var result = new StatusCalculator().CalculateBasedOnRevenueAndCloseProbability(opportunity);

            //    //Assert
            Assert.AreEqual(result.oppState.Value, expectedState);
            Assert.AreEqual(result.oppStatus.Value, expectedStatus);
        }
    }
}
