using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace DragonflyDataLibrary.Models
{
    /// <summary>
    /// A User of the Dragonfly application. 
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
        /// <summary>
        /// Unique email address of the user.
        /// </summary>
        public string EmailAddress { get; set; }
        public string PasswordHash { get; set; }
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
