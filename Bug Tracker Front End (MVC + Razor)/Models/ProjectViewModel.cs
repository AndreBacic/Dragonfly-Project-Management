using Bug_Tracker_Library;
using Bug_Tracker_Library.Models;
using System;
using System.Collections.Generic;

namespace Bug_Tracker_Front_End__MVC_plus_Razor.Models
{
    public class ProjectViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
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
        public Dictionary<Guid, UserModel> Workers { get; set; } = new();
        public string Description { get; set; }
        public List<CommentModel> Comments { get; set; } = new();
        public DateTime Deadline { get; set; }
        public ProjectStatus Status { get; set; }
        public ProjectPriority Priority { get; set; }

    }
}
