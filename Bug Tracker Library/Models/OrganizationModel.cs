using System;
using System.Collections.Generic;

namespace Bug_Tracker_Library.Models
{
    /// <summary>
    /// An organization that serves as an umbrella for a specific group of users working on their projects.
    /// </summary>
    public class OrganizationModel
    {
        public virtual Guid Id { get; set; }
        public virtual string Name { get; set; }
        /// <summary>
        /// Projects under the umbrella of this, not including these project's sub-projects.
        /// </summary>
        public virtual List<ProjectModel> Projects { get; set; }
        /// <summary>
        /// The Admin over this.
        /// </summary>
        //public UserModel Admin { get; set; } // todo: delete because admin is assigned that role and stroed in Workers?
        /// <summary>
        /// The Users assigned to work on this including the admin.
        /// </summary>
        public virtual List<UserModel> Workers { get; set; }

        /// <summary>
        /// The description of this.
        /// </summary>
        public virtual string Description { get; set; }

        public virtual string PasswordHash { get; set; }

        public ProjectModel GetProjectByIndexTree(List<int> indexTree) // todo: have projects and comments reference their parents' indexTree
        {
            if (indexTree == null || indexTree.Count == 0)
            {
                return null;
            }
            ProjectModel project = this.Projects[indexTree[0]];
            for (int i = 1; i < indexTree.Count; i++)
            {
                if (indexTree[i] >= project.SubProjects.Count)
                {
                    return null;
                }
                project = project.SubProjects[i];
            }
            return project;
        } 
    }
}