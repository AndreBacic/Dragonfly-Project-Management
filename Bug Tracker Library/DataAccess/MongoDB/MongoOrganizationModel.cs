using Bug_Tracker_Library.Models;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace Bug_Tracker_Library.DataAccess.MongoDB
{
    [BsonIgnoreExtraElements]
    public class MongoOrganizationModel : OrganizationModel
    {
        [BsonId]
        public override Guid Id { get; set; }
        /// <summary>
        /// The Ids for each user in the organization.
        /// </summary>
        public List<Guid> DbWorkerIds { get; set; } = new List<Guid>();
        public List<MongoProjectModel> DbProjects { get; set; }
        [BsonIgnore]
        public override List<UserModel> Workers => base.Workers = new List<UserModel>();
        [BsonIgnore]
        public override List<ProjectModel> Projects => base.Projects = new List<ProjectModel>();
    }
}