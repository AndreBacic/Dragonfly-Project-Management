using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace DragonflyDataLibrary.Models
{
    /// <summary>
    /// A link between a User and a Project.
    /// </summary>
    [BsonIgnoreExtraElements]
    public class AssignmentModel
    {
        /// <summary>
        /// The Id of the user this is assigned to.
        /// </summary>
        public Guid AssigneeId { get; set; }
        public Guid OrganizationId { get; set; }
        /// <summary>
        /// The Id path to the Project that Assignee is assigned to. The last id is that of the project.
        /// </summary>
        public List<Guid> ProjectIdTreePath { get; set; }
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
