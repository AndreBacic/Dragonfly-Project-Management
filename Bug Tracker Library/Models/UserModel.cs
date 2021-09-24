using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace Bug_Tracker_Library.Models
{
    /// <summary>
    /// A User of the Bug Tracker application. 
    /// <br/>
    /// NOTE: User.EmailAddress must be unique.
    /// </summary>
    [BsonIgnoreExtraElements]
    public class UserModel
    {
        [BsonId]
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string PasswordHash { get; set; }
        public List<AssignmentModel> Assignments { get; set; }
        [BsonIgnore]
        public string Name
        {
            get
            {
                return FirstName + " " + LastName;
            }
        }
    }
}
