namespace BBW.Plugins.Opportunity.Models
{
    public static class OpportunityEntity
    {
        public enum Status_OptionSet
        {
            Open = 0,
            Won = 1,
            Lost = 2
        }

        public enum StatusReason_OptionSet
        {
            InProgress = 1,
            OnHold = 2,
            Won = 3,
            Canceled = 4,
            OutSold = 5
        }
    }
}
