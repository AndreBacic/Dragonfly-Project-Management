using System;
using System.Collections.Generic;
using System.Linq;

namespace Bug_Tracker_Library.Models
{
    /// <summary>
    /// An organization that serves as an umbrella for a specific group of users working on their projects.
    /// <br />
    /// NOTE: OrganizationModel.Name must be unique.
    /// </summary>
    public class OrganizationModel
    {
        public virtual Guid Id { get; set; }
        public virtual string Name { get; set; }
        /// <summary>
        /// Projects under the umbrella of this, not including these project's sub-projects.
        /// </summary>
        public virtual List<ProjectModel> Projects { get; set; } = new List<ProjectModel>();
        /// <summary>
        /// The Users assigned to work on this including the admin.
        /// </summary>
        public virtual List<UserModel> Workers { get; set; } = new List<UserModel>();

        /// <summary>
        /// The description of this.
        /// </summary>
        public virtual string Description { get; set; }
        /// <summary>
        /// Hashed password user must enter to access this.
        /// </summary>
        public virtual string PasswordHash { get; set; }

        public virtual ProjectModel GetProjectByIdTree(List<Guid> idTree)
        {
            if (idTree == null || idTree.Count == 0)
            {
                return null;
            }
            ProjectModel project = this.Projects.FirstOrDefault(p => p.Id == idTree[0]);
            for (int i = 1; i < idTree.Count; i++)
            {
                if (project == null)
                {
                    return null;
                }
                project = project.SubProjects.FirstOrDefault(p => p.Id == idTree[i]);
            }
            return project;
        }
    }
}