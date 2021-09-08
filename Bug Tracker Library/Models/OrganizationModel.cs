using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace Bug_Tracker_Library.Models
{
    /// <summary>
    /// An organization that serves as an umbrella for a specific group of users working on their projects.
    /// </summary>
    public class OrganizationModel
    {
        [BsonId]
        public Guid Id { get; set; }
        public string Name { get; set; }
        /// <summary>
        /// Projects under the umbrella of this, not including these project's sub-projects.
        /// </summary>
        public List<ProjectModel> Projects { get; set; }
        /// <summary>
        /// The Admin over this.
        /// </summary>
        public UserModel Admin { get; set; } // todo: delete because admin is assigned that role and stroed in Workers?
        /// <summary>
        /// The Users assigned to work on this including the admin.
        /// </summary>
        public List<UserModel> Workers { get; set; }

        /// <summary>
        /// The description of this.
        /// </summary>
        public string Description { get; set; }

        public string PasswordHash { get; set; }

    }
}
