using System;
using System.Collections.Generic;
using FakeXrmEasy;
using Microsoft.Xrm.Sdk;
using NUnit.Framework;

namespace BBW.Plugins.Tests
{
    [TestFixture()]
    public class PreUpdateTests
    {
        XrmFakedContext _fakeContext;
        [SetUp]
        public void SetUp()
        {
            _fakeContext = new XrmFakedContext();
        }

        [Test]
        public void Execute_ShouldNotFail_WhenNoDataPassedInContextEntity()
        {
            // Arrange
            var opportunity = new Entity("opportunity", Guid.NewGuid());
            var preImage = new Entity("opportunity", opportunity.Id);

            var plugCtx = _fakeContext.GetDefaultPluginContext();
            plugCtx.MessageName = "Update";
            plugCtx.InputParameters = new ParameterCollection
            {
                { "Target", opportunity }
            };
            plugCtx.PreEntityImages = new EntityImageCollection()
            {
                new KeyValuePair<string, Entity>("PreImage", preImage)
            };

            // Act & Assert
            Assert.DoesNotThrow(() => _fakeContext.ExecutePluginWith<PreUpdate>(plugCtx));
        }

        [Test]
        public void Execute_ShouldNotFail_WhenNoEntityPassedInContext()
        {
            // Arrange
            var opportunity = new Entity("opportunity", Guid.NewGuid());
            var preImage = new Entity("opportunity", opportunity.Id);

            var plugCtx = _fakeContext.GetDefaultPluginContext();
            plugCtx.MessageName = "Update";
            plugCtx.PreEntityImages = new EntityImageCollection()
            {
                new KeyValuePair<string, Entity>("PreImage", preImage)
            };

            // Act & Assert
            Assert.DoesNotThrow(() => _fakeContext.ExecutePluginWith<PreUpdate>(plugCtx));
        }

        [Test]
        public void Execute_ShouldFail_WhenNoDataPassedInPreImage()
        {
            // Arrange
            var opportunity = new Entity("opportunity", Guid.NewGuid());
            opportunity["closeprobability"] = new Random().Next(0, 100);

            var preImage = new Entity("opportunity", opportunity.Id);

            var plugCtx = _fakeContext.GetDefaultPluginContext();
            plugCtx.MessageName = "Update";
            plugCtx.InputParameters = new ParameterCollection
            {
                { "Target", opportunity }
            };
            plugCtx.PreEntityImages = new EntityImageCollection()
            {
                new KeyValuePair<string, Entity>("PreImage", preImage)
            };

            // Act & Assert
            Assert.Throws<NullReferenceException>(() => _fakeContext.ExecutePluginWith<PreUpdate>(plugCtx));
        }

    }
}
