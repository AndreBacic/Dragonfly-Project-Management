using System;
using System.Collections.Generic;

namespace Bug_Tracker_Library.Models
{
    /// <summary>
    /// A link between a User and a Project.
    /// </summary>
    public class AssignmentModel
    {
        /// <summary>
        /// The Id of the user this is assigned to.
        /// </summary>
        public virtual Guid AssigneeId { get; set; }
        public virtual Guid OrganizationId { get; set; }
        /// <summary>
        /// The Id path to the Project that Assignee is assigned to. The last id is that of the project.
        /// </summary>
        public virtual List<Guid> ProjectIdTreePath { get; set; } = new List<Guid>();
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
