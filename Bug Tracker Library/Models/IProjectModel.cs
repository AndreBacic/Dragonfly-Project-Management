using System;
using System.Collections.Generic;

namespace Bug_Tracker_Library.Models
{
    /// <summary>
    /// A Project to be tracked and managed by the appliction.
    /// </summary>
    public interface IProjectModel
    {
        /// <summary>
        /// Unique identifier for the project.
        /// </summary>
        Guid Id { get; set; }
        string Name { get; set; }

        /// <summary>
        /// If this is a subproject, ParentIdTreePath is a list of the IDs to navigate the project tree to this's parent project.
        /// (this.Id is not included in the path)
        /// <br></br>
        /// If this is a top level project, then ParentIdTreePath is empty.
        /// </summary>
        List<Guid> ParentIdTreePath { get; set; }

        /// <summary>
        /// Lesser projects under the umbrella of this.
        /// <br/>
        /// Do not directly add projects to this list; use AddSubProject() so that
        /// the ParentIdTreePath is automatically maintained.
        /// </summary>
        List<IProjectModel> SubProjects { get; set; }
        /// <summary>
        /// The users assigned to work on this.
        /// </summary>
        List<IUserModel> Workers { get; set; }
        /// <summary>
        /// The description of this, which may be just a short statement or a very long list of instructions to Workers.
        /// </summary>
        string Description { get; set; }
        /// <summary>
        /// All comments that the Workers have posted to this.
        /// <br />
        /// Do not directly add comments to this list; use AddComment() so that
        /// the ParentIdTreePath is automatically added to the comment.
        /// </summary>
        List<ICommentModel> Comments { get; set; }
        /// <summary>
        /// The date that all work on this must be done.
        /// </summary>
        DateTime Deadline { get; set; }
        ProjectStatus Status { get; set; }
        ProjectPriority Priority { get; set; }

        void AddSubProject(IProjectModel newProject)
        {
            if (newProject == null)
            {
                return;
            }
            newProject.ParentIdTreePath = new List<Guid>(this.ParentIdTreePath)
                {
                    this.Id
                };
            this.SubProjects.Add(newProject);
        }

        void AddComment(ICommentModel model)
        {
            if (model == null)
            {
                return;
            }
            model.ParentProjectIdTreePath = new List<Guid>(this.ParentIdTreePath)
                {
                    this.Id
                };
            this.Comments.Add(model);
        }
    }
}
