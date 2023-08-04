using System.Collections.Generic;

namespace BBW.Plugins.Opportunity
{
    public static class Constants
    {
        public static List<string> AllOpportunitiesAttributes = new List<string>()
        {
            "statecode",
            "statuscode",
            "estimatedvalue",
            "closeprobability"
        };
        public const int AmountToWin = 100000;
        public const int CloseProbabilityToWin = 80;
    }
}
