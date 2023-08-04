using BBW.Plugins.Opportunity.Services;
using Microsoft.Xrm.Sdk;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace BBW.Plugins.Tests.Services
{
    [TestFixture]
    public class EntityServiceTests
    {
        Guid _opportunityId = Guid.NewGuid();
        [Test]
        public void MergeEntities_ShouldNotModifyMainEntity_WhenOldEntityIsNull()
        {
            // Arrange
            Entity mainEntity = new Entity("opportunity", _opportunityId);
            mainEntity["new_name"] = "Test Opportunity";
            List<string> attributes = new List<string>() { "new_name" };

            // Act
            Entity mergedEntity = EntityService.MergeEntities(mainEntity, null, attributes);

            // Assert
            Assert.AreEqual(mainEntity.Id, mergedEntity.Id);
            Assert.AreEqual(mainEntity.Attributes.Count, mergedEntity.Attributes.Count);
            Assert.AreEqual(mainEntity.GetAttributeValue<string>("new_name"), mergedEntity.GetAttributeValue<string>("new_name"));
        }

        [Test]
        public void MergeEntities_ShouldReturnValueFromMainEntity_WhenFieldExistsInBothEntities()
        {
            // Arrange
            Entity mainEntity = new Entity("opportunity", _opportunityId);
            mainEntity["new_name"] = "Test Opportunity";
            Entity oldEntity = new Entity("opportunity", _opportunityId);
            oldEntity["new_name"] = "Test Old Opportunity";
            List<string> attributes = new List<string>() { "new_name" };

            // Act
            Entity mergedEntity = EntityService.MergeEntities(mainEntity, oldEntity, attributes);

            // Assert
            Assert.AreEqual(mainEntity.GetAttributeValue<string>("new_name"), mergedEntity.GetAttributeValue<string>("new_name"));
        }

        [Test]
        public void MergeEntities_ShouldAddMissingAttribute_WhenItExistsInOldEntity()
        {
            // Arrange
            var description = "Test Old Opportunity Description";
            Entity mainEntity = new Entity("opportunity", _opportunityId);
            mainEntity["new_name"] = "Test Opportunity";

            Entity oldEntity = new Entity("opportunity", _opportunityId);
            oldEntity["new_name"] = "Test Opportunity OLD";
            oldEntity["new_description"] = description;
            List<string> attributes = new List<string>() { "new_name", "new_description" };

            // Act
            Entity mergedEntity = EntityService.MergeEntities(mainEntity, oldEntity, attributes);

            // Assert
            Assert.AreEqual(description, mergedEntity.GetAttributeValue<string>("new_description"));
            Assert.AreEqual(mainEntity.GetAttributeValue<string>("new_name"), mergedEntity.GetAttributeValue<string>("new_name"));
        }
    }
}
