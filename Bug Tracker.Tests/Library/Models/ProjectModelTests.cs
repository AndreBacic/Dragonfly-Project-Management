using Bug_Tracker_Library;
using Bug_Tracker_Library.Models;
using System;
using System.Numerics;
using Xunit;

namespace Bug_Tracker.Library.Models.Tests
{
    public class IProjectModelTests
    {
        [Fact]
        public void NewIProjectModel_ShouldMakeRandomGuidId()
        {
            // Arrange
            Guid emptyGuid = new Guid();
            // Act
            IProjectModel model1 = new IProjectModel();
            IProjectModel model2 = new IProjectModel()
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
            IProjectModel mainProject = new IProjectModel()
            {
                Name = "Main",
                Priority = ProjectPriority.TOP,
                Status = ProjectStatus.IN_PROGRESS
            };
            IProjectModel subProject = new IProjectModel()
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
            decimal guidPossibilities = decimal.MaxValue;
            BigInteger actualGuidPossibilities = BigInteger.Pow(2, 128);
            // factorOff == 4294967296 == 2 ** 32
            BigInteger factorOff = BigInteger.Divide(actualGuidPossibilities, (BigInteger)guidPossibilities);
            decimal odds = 1.0m;
            for (int i = 0; i < numProjects; i++)
            {
                decimal b = Decimal.Subtract(guidPossibilities, i);
                decimal a = Decimal.Divide(b, guidPossibilities);
                odds = Decimal.Multiply(odds, a);
            }

            decimal oddsCollision = Decimal.Subtract(1.0m, odds);
            decimal actualOdds = Decimal.Divide(oddsCollision, (decimal)factorOff);

            Assert.True(odds > 0.9999_9999_9999m);
            Assert.True(actualOdds == 0.0m);
        }
    }
}
