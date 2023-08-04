using System;
using BBW.Plugins.Opportunity.Services;
using Microsoft.Xrm.Sdk;

namespace BBW.Plugins
{
    public class PreUpdate : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            var tracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));
            var context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            if (context.InputParameters.Contains("Target") && context.InputParameters["Target"] is Entity)
            {
                var opportunity = context.InputParameters["Target"] as Entity;
                var preImage = (Entity)context.PreEntityImages["PreImage"];
                var serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
                var orgService = serviceFactory.CreateOrganizationService(context.UserId);
                try
                {
                    var statusService = new SetStatusService(tracingService, new StatusCalculator());
                    statusService.SetStatus(opportunity, preImage);
                }
                catch (Exception ex)
                {
                    tracingService.Trace("BBW.CRM.Plugins.Opportunity.PreUpdate: " + ex.ToString());
                    throw ex;
                }
            }
        }
    }
}
