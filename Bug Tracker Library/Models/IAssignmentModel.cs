using System;
using System.Collections.Generic;

namespace Bug_Tracker_Library.Models
{
    /// <summary>
    /// A link between a User and a Project.
    /// </summary>
    public interface IAssignmentModel
    {
        /// <summary>
        /// The Id of the user this is assigned to.
        /// </summary>
        Guid AssigneeId { get; set; }
        Guid OrganizationId { get; set; }
        /// <summary>
        /// The Id path to the Project that Assignee is assigned to. The last id is that of the project.
        /// </summary>
        List<Guid> ProjectIdTreePath { get; set; }
        /// <summary>
        /// Position (level of access) Assignee has to Project.
        /// </summary>
        UserPosition AssigneeAccess { get; set; }
        /// <summary>
        /// Hours of work Assignee has spent on Project.
        /// </summary>
        double HoursLogged { get; set; }
    }
}
