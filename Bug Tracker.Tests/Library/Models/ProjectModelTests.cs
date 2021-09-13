using Bug_Tracker_Library;
using Bug_Tracker_Library.Models;
using System;
using System.Numerics;
using Xunit;

namespace Bug_Tracker.Library.Models.Tests
{
    public class ProjectModelTests
    {
        [Fact]
        public void NewProjectModel_ShouldMakeRandomGuidId()
        {
            // Arrange
            Guid emptyGuid = new Guid();
            // Act
            ProjectModel model1 = new ProjectModel();
            ProjectModel model2 = new ProjectModel()
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
            ProjectModel mainProject = new ProjectModel()
            {
                Name = "Main",
                Priority = ProjectPriority.TOP,
                Status = ProjectStatus.IN_PROGRESS
            };
            ProjectModel subProject = new ProjectModel()
            {
                Name = "sub",
                Priority = ProjectPriority.MEDIUM,
                Status = ProjectStatus.TODO
            };

            mainProject.AddSubProject(subProject);

            Assert.Contains(subProject, mainProject.SubProjects);
            Assert.Contains(mainProject.Id, subProject.ParentIdTreePath);
        }

        [Theory]
        [InlineData(10)]
        [InlineData(1000)]
        [InlineData(100000)]
        public void ProjectIds_ShouldntCollide(int numProjects)
        {
            var guidPossibilities = decimal.MaxValue;
            var actualGuidPossibilities = BigInteger.Pow(2, 128);
            // factorOff == 4294967296 == 2 ** 32
            var factorOff = BigInteger.Divide(actualGuidPossibilities, (BigInteger)guidPossibilities);
            decimal odds = 1.0m;
            for (int i = 0; i < numProjects; i++)
            {
                var b = Decimal.Subtract(guidPossibilities, i);
                var a = Decimal.Divide(b, guidPossibilities);
                odds = Decimal.Multiply(odds, a);
            }

            var oddsCollision = Decimal.Subtract(1.0m, odds);
            var actualOdds = Decimal.Divide(oddsCollision, (decimal)factorOff);

            Assert.True(odds > 0.9999_9999_9999m);
            Assert.True(actualOdds == 0.0m);
        }
    }
}
