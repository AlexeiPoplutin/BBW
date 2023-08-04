using Microsoft.Xrm.Sdk;
using BBW.Plugins.Opportunity.Interfaces;

namespace BBW.Plugins.Opportunity.Services
{
    public class SetStatusService
    {
        private readonly ITracingService _tracing;
        private readonly IStatusCalculator _statusCalculator;

        public SetStatusService(ITracingService tracing, IStatusCalculator statusCalculator)
        {
            _tracing = tracing;
            _statusCalculator = statusCalculator;
        }

        public void SetStatus(Entity opportunityFromCtx, Entity preImage)
        {
            _tracing.Trace("Start SetStatus");
            if (!ShouldServiceBeExecuted(opportunityFromCtx)) return;

            var mergedOpportunity = EntityService.MergeEntities(opportunityFromCtx, preImage, Constants.AllOpportunitiesAttributes);

            var calculatedStatus = _statusCalculator.CalculateBasedOnRevenueAndCloseProbability(mergedOpportunity);

            if (isStatusUpdateNeeded(calculatedStatus.oppState, calculatedStatus.oppStatus, mergedOpportunity))
            {
                _tracing.Trace("Updating status.");
                opportunityFromCtx["statecode"] = calculatedStatus.oppState;
                opportunityFromCtx["statuscode"] = calculatedStatus.oppStatus;
            }
        }

        private bool ShouldServiceBeExecuted(Entity opportunityFromCtx)
        {
            return opportunityFromCtx.Contains("closeprobability") || opportunityFromCtx.Contains("estimatedvalue");
        }

        private bool isStatusUpdateNeeded(OptionSetValue oppState, OptionSetValue oppStatus, Entity mergedOpportunity)
        {
            var currentStatus = mergedOpportunity.GetAttributeValue<OptionSetValue>("statuscode");
            var currentState = mergedOpportunity.GetAttributeValue<OptionSetValue>("statecode");

            return oppStatus != null && oppState != null
                && (currentStatus?.Value != oppStatus.Value || currentState?.Value != oppState.Value);
        }
    }
}