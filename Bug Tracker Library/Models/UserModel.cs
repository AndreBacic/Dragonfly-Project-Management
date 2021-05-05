using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bug_Tracker_Library.Models
{
    /// <summary>
    /// A User of the Bug Tracker application. 
    /// </summary>
    public class UserModel
    {
        public int Id { get; set; }
        [BsonId]
        public Guid GuidId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string PasswordHash { get; set; }

        /// <summary>
        /// Tasks assigned to the User.
        /// </summary>
        public List<AssignmentModel> Assignments { get; set; }

        public string Name 
        { get
            {
                return this.FirstName + " " + this.LastName;
            } 
        }
    }
}
