using System.Collections.Generic;
using Microsoft.Xrm.Sdk;

namespace BBW.Plugins.Opportunity.Services
{
    public class EntityService
    {
        public static Entity MergeEntities(Entity mainEntity, Entity oldEntity, List<string> attributes)
        {
            Entity mergedEntity = new Entity(mainEntity.LogicalName, mainEntity.Id);
            foreach (var attr in mainEntity.Attributes)
            {
                mergedEntity[attr.Key] = attr.Value;
            }

            if (oldEntity == null)
                return mergedEntity;

            foreach (var item in attributes)
            {
                if (!mergedEntity.Contains(item) && oldEntity.Contains(item))
                {
                    mergedEntity[item] = oldEntity[item];
                }
            }

            return mergedEntity;
        }
    }
}
