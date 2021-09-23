using Bug_Tracker_Library.Models;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace Bug_Tracker_Library.DataAccess.MongoDB
{
    [BsonIgnoreExtraElements]
    public class MongoCommentModel : ICommentModel
    {
        public Guid PosterId { get; set; }
        [BsonIgnore]
        public IUserModel Poster { get; set; }
        public DateTime DatePosted { get; set; }
        public string Text { get; set; }
        public List<Guid> ParentProjectIdTreePath { get; set; }
    }
}