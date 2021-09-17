using Bug_Tracker_Library.Models;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Bug_Tracker_Library.DataAccess.MongoDB
{
    [BsonIgnoreExtraElements]
    public class MongoCommentModel : CommentModel
    {
        public Guid PosterId { get; set; }
        [BsonIgnore]
        public override UserModel Poster => base.Poster;
    }
}