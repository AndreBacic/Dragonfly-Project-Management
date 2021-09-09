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
        public List<MongoUserModel> DbWorkers { get; set; }
        [BsonIgnore]
        public override List<UserModel> Workers { get => base.Workers; set => base.Workers = value; }
    }
}