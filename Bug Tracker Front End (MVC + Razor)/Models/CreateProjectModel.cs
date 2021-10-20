using Bug_Tracker_Library;
using System;
using System.Collections.Generic;

namespace Bug_Tracker_Front_End__MVC_plus_Razor.Models
{
    public class CreateProjectModel
    {
        public string Name { get; set; }

        /// <summary>
        /// If this is a subproject, ParentIdTreePath is a list of the IDs to navigate the project tree to this's parent project.
        /// (this.Id is not included in the path)
        /// <br></br>
        /// If this is a top level project, then ParentIdTreePath is empty.
        /// </summary>
        public List<Guid> ParentIdTreePath { get; set; } = new();
        /// <summary>
        /// The description of this, which may be just a short statement or a very long list of instructions to Workers.
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// The date that all work on this must be done.
        /// </summary>
        public DateTime Deadline { get; set; }
        public ProjectStatus Status { get; set; }
        public ProjectPriority Priority { get; set; }
    }
}
