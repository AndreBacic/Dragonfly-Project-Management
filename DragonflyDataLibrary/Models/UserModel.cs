﻿using MongoDB.Bson.Serialization.Attributes;
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
        // TODO: Decide if this app will one day allow for social networking, or if the unnecessary name fields should be deleted.
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [BsonIgnore]
        public string Name => $"{FirstName} {LastName}";
        /// <summary>
        /// Unique email address of the user.
        /// </summary>
        public string EmailAddress { get; set; }
        public string PasswordHash { get; set; }
        public List<ProjectModel> Projects { get; set; } = new();
        public ColorPreference ColorPreference { get; set; }
    }
}
