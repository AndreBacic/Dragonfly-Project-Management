using Bug_Tracker_Library.Models;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Bug_Tracker_Library.DataAccess.MongoDB
{
    [BsonIgnoreExtraElements]
    public class MongoUserModel : UserModel
    {
        [BsonId]
        public override Guid Id { get; set; }
    }
}
