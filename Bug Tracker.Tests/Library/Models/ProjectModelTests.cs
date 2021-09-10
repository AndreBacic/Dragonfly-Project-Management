using Bug_Tracker_Library.Models;
using System;
using Xunit;

namespace Bug_Tracker.Library.Models.Tests
{
    public class ProjectModelTests
    {
        [Fact]
        public void NewProjectModelShouldMakeRandomGuidId()
        {
            // Arrange
            var emptyGuid = new Guid();
            // Act
            var model1 = new ProjectModel();
            var model2 = new ProjectModel()
            {
                Name = "Model2"
            };
            // Assert
            Assert.True(Guid.Equals(emptyGuid, Guid.Empty));
            Assert.False(Guid.Equals(model1.Id, emptyGuid));
            Assert.False(Guid.Equals(model2.Id, emptyGuid));
            Assert.False(Guid.Equals(model2.Id, model1.Id));
        }
    }
}
