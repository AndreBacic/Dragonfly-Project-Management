using System;
using System.Collections.Generic;

namespace Bug_Tracker_Library.Models
{
    /// <summary>
    /// A Project to be tracked and managed by the appliction.
    /// </summary>
    public class ProjectModel
    {
        /// <summary>
        /// Unique identifier for the project.
        /// </summary>
        public virtual Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }

        /// <summary>
        /// If this is a subproject, ParentIdTreePath is a list of the IDs to navigate the project tree to this's parent project.
        /// (this.Id is not included in the path)
        /// <br></br>
        /// If this is a top level project, then ParentIdTreePath is null.
        /// </summary>
        public virtual List<Guid> ParentIdTreePath { get; set; } = null;

        /// <summary>
        /// Lesser projects under the umbrella of this.
        /// <br/>
        /// Do not directly add projects to this list; use AddSubProject() so that
        /// the ParentIdTreePath is automatically maintained.
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
            List<Guid> treeWithSub = new List<Guid>(this.ParentIdTreePath)
            {
                this.Id
            };
            newProject.ParentIdTreePath = treeWithSub;
            this.SubProjects.Add(newProject);
        }
    }
}
