using BBW.Plugins.Opportunity.Interfaces;
using BBW.Plugins.Opportunity.Models;
using Microsoft.Xrm.Sdk;

namespace BBW.Plugins.Opportunity.Services
{
    public class StatusCalculator : IStatusCalculator
    {
        public (OptionSetValue oppState, OptionSetValue oppStatus) CalculateBasedOnRevenueAndCloseProbability(Entity mergedOpportunity)
        {
            OptionSetValue oppStatus, oppState;
            var closeProbab = mergedOpportunity.GetAttributeValue<int?>("closeprobability");
            var estimatedVal = mergedOpportunity.GetAttributeValue<Money>("estimatedvalue");

            if ((closeProbab.HasValue && closeProbab == 0)
                || (estimatedVal != null && estimatedVal?.Value == 0))
            {
                oppState = new OptionSetValue((int)OpportunityEntity.Status_OptionSet.Lost);
                oppStatus = new OptionSetValue((int)OpportunityEntity.StatusReason_OptionSet.Canceled);
            }
            else if (closeProbab >= Constants.CloseProbabilityToWin && estimatedVal?.Value >= Constants.AmountToWin)
            {
                oppState = new OptionSetValue((int)OpportunityEntity.Status_OptionSet.Won);
                oppStatus = new OptionSetValue((int)OpportunityEntity.StatusReason_OptionSet.Won);
            }
            else
            //if (!closeProbab.HasValue || estimatedVal == null
            //    || (closeProbab > 0 && closeProbab < CloseProbabilityToWin && estimatedVal.Value > 0 && estimatedVal.Value < AmountToWin))
            {
                oppState = new OptionSetValue((int)OpportunityEntity.Status_OptionSet.Open);
                oppStatus = new OptionSetValue((int)OpportunityEntity.StatusReason_OptionSet.InProgress);
            }

            return (oppState, oppStatus);
        }
    }
}
