using Bug_Tracker_Library.Models;
using System;
using System.Collections.Generic;
using Xunit;

namespace Bug_Tracker.Library.Models.Tests
{
    public class OrganizatioModelTests
    {
        [Fact]
        public void GetGetProjectByIdTree_ShouldWork()
        {
            OrganizationModel org = new OrganizationModel();
            org.Projects.Add(new ProjectModel() { Name = "George" });
            org.Projects[0].AddSubProject(new ProjectModel() { Name = "John" });
            ProjectModel p = new ProjectModel() { Name = "Fred" };
            org.Projects[0].SubProjects[0].AddSubProject(p);

            List<Guid> path = new List<Guid>(p.ParentIdTreePath) { p.Id };

            ProjectModel p2 = org.GetProjectByIdTree(path);

            Assert.Equal(p, p2);
            Assert.Equal(org.Projects[0].SubProjects[0], org.GetProjectByIdTree(p.ParentIdTreePath));
        }
    }
}
