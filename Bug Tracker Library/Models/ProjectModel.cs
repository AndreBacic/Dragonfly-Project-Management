using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace Bug_Tracker_Library.Models
{
    /// <summary>
    /// A Project to be tracked and managed by the appliction.
    /// </summary>
    [BsonIgnoreExtraElements]
    public class ProjectModel
    {
        /// <summary>
        /// Unique identifier for the project.
        /// </summary>
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }

        /// <summary>
        /// If this is a subproject, ParentIdTreePath is a list of the IDs to navigate the project tree to this's parent project.
        /// (this.Id is not included in the path)
        /// <br></br>
        /// If this is a top level project, then ParentIdTreePath is empty.
        /// </summary>
        public List<Guid> ParentIdTreePath { get; set; } = new();

        /// <summary>
        /// Lesser projects under the umbrella of this.
        /// <br/>
        /// Do not directly add projects to this list; use AddSubProject() so that
        /// the ParentIdTreePath is automatically maintained.
        /// </summary>
        public List<ProjectModel> SubProjects { get; set; } = new();
        /// <summary>
        /// The ids of the users assigned to work on this.
        /// </summary>
        public List<Guid> WorkerIds { get; set; }
        /// <summary>
        /// The description of this, which may be just a short statement or a very long list of instructions to Workers.
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// All comments that the Workers have posted to this.
        /// <br />
        /// Do not directly add comments to this list; use AddComment() so that
        /// the ParentIdTreePath is automatically added to the comment.
        /// </summary>
        public List<CommentModel> Comments { get; set; }
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
            newProject.ParentIdTreePath = new List<Guid>(this.ParentIdTreePath)
                {
                    this.Id
                };
            this.SubProjects.Add(newProject);
        }

        public void AddComment(CommentModel model)
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
