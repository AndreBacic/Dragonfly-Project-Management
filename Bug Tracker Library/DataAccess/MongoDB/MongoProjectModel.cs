using Bug_Tracker_Library.Models;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace Bug_Tracker_Library.DataAccess.MongoDB
{
    [BsonIgnoreExtraElements]
    public class MongoProjectModel : IProjectModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<Guid> ParentIdTreePath { get; set; }
        public List<IProjectModel> SubProjects { get; set; }
        public List<Guid> WorkerIds { get; set; }
        [BsonIgnore]
        public List<IUserModel> Workers { get; set; }
        public string Description { get; set; }
        public List<ICommentModel> Comments { get; set; }
        public DateTime Deadline { get; set; }
        public ProjectStatus Status { get; set; }
        public ProjectPriority Priority { get; set; }
    }
}