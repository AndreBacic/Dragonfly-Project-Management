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
            IOrganizationModel org = new IOrganizationModel();
            org.Projects.Add(new IProjectModel() { Name = "George" });
            org.Projects[0].AddSubProject(new IProjectModel() { Name = "John" });
            IProjectModel p = new IProjectModel() { Name = "Fred" };
            org.Projects[0].SubProjects[0].AddSubProject(p);

            List<Guid> path = new List<Guid>(p.ParentIdTreePath) { p.Id };

            IProjectModel p2 = org.GetProjectByIdTree(path);

            Assert.Equal(p, p2);
            Assert.Equal(org.Projects[0].SubProjects[0], org.GetProjectByIdTree(p.ParentIdTreePath));
        }
    }
}
