using System;
using System.Collections.Generic;
using System.Text;

namespace Bug_Tracker_Library.Models
{
    /// <summary>
    /// A Project to be tracked and managed by the appliction.
    /// </summary>
    class Project
    {
        public int Id { get; set; }
        public string Name { get; set; }
        /// <summary>
        /// If this is a sub-project of a larger one, the larger one is ParentProject.
        /// If this is at the top of a project heirarchy, then ParentProject will just remain null.
        /// </summary>
        public Project ParentProject { get; set; }
        /// <summary>
        /// Lesser projects under the umbrella of this.
        /// </summary>
        public List<Project> SubProjects { get; set; }
        /// <summary>
        /// The Users assigned to work on this.
        /// </summary>
        public List<User> Workers { get; set; }
        /// <summary>
        /// The description of this, which may be just a short statement or a very long list of instructions to Workers.
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// All comments that the Workers have posted to this.
        /// </summary>
        public List<Comment> Comments { get; set; }
        /// <summary>
        /// The date that all work on this must be done.
        /// </summary>
        //[DataType(DataType.Date)] // I don't know if this is needed, but in SQL Deadline is saved as DATETIME.
        public string Deadline { get; set; }
        public ProjectStatus Status { get; set; }
        public ProjectPriority Priority { get; set; }
    }
}
