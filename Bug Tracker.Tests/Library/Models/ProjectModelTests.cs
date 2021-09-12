using Bug_Tracker_Library;
using Bug_Tracker_Library.Models;
using System;
using Xunit;

namespace Bug_Tracker.Library.Models.Tests
{
    public class ProjectModelTests
    {
        [Fact]
        public void NewProjectModel_ShouldMakeRandomGuidId()
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

        [Fact]
        public void AddSubProject_ShouldWork()
        {
            var mainProject = new ProjectModel()
            {
                Name = "Main",
                Priority = ProjectPriority.TOP,
                Status = ProjectStatus.IN_PROGRESS
            };
            var subProject = new ProjectModel()
            {
                Name = "sub",
                Priority = ProjectPriority.MEDIUM,
                Status = ProjectStatus.TODO
            };

            mainProject.AddSubProject(subProject);

            Assert.Contains(subProject, mainProject.SubProjects);
            Assert.Contains(mainProject.Id, subProject.ParentIdTreePath);
        }
    }
}
