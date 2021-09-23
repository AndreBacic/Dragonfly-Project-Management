using Bug_Tracker_Library.Models;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bug_Tracker_Library.DataAccess.MongoDB
{
    [BsonIgnoreExtraElements]
    public class MongoOrganizationModel : IOrganizationModel
    {
        [BsonId]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<IProjectModel> Projects { get; set; }
        public List<Guid> WorkerIds { get; set; }
        [BsonIgnore]
        public List<IUserModel> Workers { get; set; }
        public string Description { get; set; }
    }
}