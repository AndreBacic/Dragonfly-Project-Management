using System;
using System.Collections.Generic;

namespace Bug_Tracker_Library.Models
{
    /// <summary>
    /// A Project to be tracked and managed by the appliction.
    /// </summary>
    public class ProjectModel
    {
        public string Name { get; set; }

        /// <summary>
        /// If this is a subproject, ParentIndexTree is a list of the indecies to navigate the project tree to this
        /// (the last index is the index of this)
        /// </summary>
        public List<int> ParentIndexTree { get; set; }

        /// <summary>
        /// Lesser projects under the umbrella of this.
        /// </summary>
        public virtual List<ProjectModel> SubProjects { get; private set; } = new List<ProjectModel>();
        /// <summary>
        /// The Users assigned to work on this.
        /// </summary>
        public virtual List<UserModel> Workers { get; set; }
        /// <summary>
        /// The description of this, which may be just a short statement or a very long list of instructions to Workers.
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// All comments that the Workers have posted to this.
        /// </summary>
        public virtual List<CommentModel> Comments { get; set; }
        /// <summary>
        /// The date that all work on this must be done.
        /// </summary>
        public DateTime Deadline { get; set; }
        public ProjectStatus Status { get; set; }
        public ProjectPriority Priority { get; set; }

        public void AddSubProject(ProjectModel newProject)
        {
            if (newProject == null)
            {
                return;
            }
            List<int> treeWithSub = new List<int>(this.ParentIndexTree)
            {
                this.SubProjects.Count
            };
            newProject.ParentIndexTree = treeWithSub;
            this.SubProjects.Add(newProject);
        }
    }
}
