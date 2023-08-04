using Microsoft.Xrm.Sdk;

namespace BBW.Plugins.Opportunity.Interfaces
{
    public interface IStatusCalculator
    {
        (OptionSetValue oppState, OptionSetValue oppStatus) CalculateBasedOnRevenueAndCloseProbability(Entity opportunity);
    }
}
