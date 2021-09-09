using Bug_Tracker_Library.Models;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace Bug_Tracker_Library.DataAccess.MongoDB
{
    public class MongoOrganizationModel : OrganizationModel
    {
        [BsonId]
        public override Guid Id { get; set; }
        public List<MongoUserModel> DbWorkers { get; set; }
    }
}