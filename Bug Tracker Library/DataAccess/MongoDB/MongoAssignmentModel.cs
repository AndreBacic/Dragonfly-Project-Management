using Bug_Tracker_Library.Models;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace Bug_Tracker_Library.DataAccess.MongoDB
{
    [BsonIgnoreExtraElements]
    public class MongoAssignmentModel : IAssignmentModel
    {
        public Guid AssigneeId { get; set; }
        public Guid OrganizationId { get; set; }
        public List<Guid> ProjectIdTreePath { get; set; }
        public UserPosition AssigneeAccess { get; set; }
        public double HoursLogged { get; set; }
    }
}