using Bug_Tracker_Library.Models;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace Bug_Tracker_Library.DataAccess.MongoDB
{
    [BsonIgnoreExtraElements]
    public class MongoProjectModel : ProjectModel
    {
        public List<Guid> DbWorkerIds { get; set; }
        [BsonIgnore]
        public override List<UserModel> Workers => base.Workers = new List<UserModel>();

        public List<MongoProjectModel> DbSubProjects { get; set; } = new List<MongoProjectModel>();
        [BsonIgnore]
        public override List<ProjectModel> SubProjects => base.SubProjects = new List<ProjectModel>();

        public List<MongoCommentModel> DbComments { get; set; }
        [BsonIgnore]
        public override List<CommentModel> Comments => base.Comments = new List<CommentModel>();
    }
}