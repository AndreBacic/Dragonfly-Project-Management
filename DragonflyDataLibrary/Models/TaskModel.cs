using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace DragonflyDataLibrary.Models
{
    [BsonIgnoreExtraElements]
    public class TaskModel
    {
        public int Id { get; set; } // = Guid.NewGuid(); // TODO: choose to use Guid or int for project and task IDs
        public string Title { get; set; }
        public string Details { get; set; }
        public DateTime Created { get; set; }
        public DateTime Deadline { get; set; }
        public decimal Cost { get; set; }
        public TaskType Type { get; set; }
        public TaskStatus Status { get; set; }
        public TaskPriority Priority { get; set; }
        public List<TaskModel> ChildTasks { get; set; }
    }
}
