using System;

namespace Bug_Tracker_Library.Models
{
    /// <summary>
    /// A link between a User and a Project.
    /// </summary>
    public class AssignmentModel
    {
        /// <summary>
        /// The id of the user this is assigned to.
        /// </summary>
        public Guid AssigneeId { get; set; }
        /// <summary>
        /// The Project that Assignee is assigned to.
        /// </summary>
        public virtual ProjectModel Project { get; set; }
        /// <summary>
        /// Position (level of access) Assignee has to Project.
        /// </summary>
        public UserPosition AssigneeAccess { get; set; }
        /// <summary>
        /// Hours of work Assignee has spent on Project.
        /// </summary>
        public double HoursLogged { get; set; }
    }
}
