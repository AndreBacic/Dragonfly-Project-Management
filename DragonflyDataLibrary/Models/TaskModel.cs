using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace DragonflyDataLibrary.Models
{
    [BsonIgnoreExtraElements]
    public class TaskModel
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; } = "";
        public string Details { get; set; } = "";
        public DateTime Created { get; set; }
        public DateTime Deadline { get; set; }
        public decimal Cost { get; set; }
        public TaskType Type { get; set; }
        public TaskStatus Status { get; set; }
        public TaskPriority Priority { get; set; }

        /// <summary>
        /// The ID of the parent task this task belongs to
        /// </summary>
        //public Guid? ParentId { get; set; }
        /// <summary>
        /// The IDs of other tasks in this's project that are 
        /// nested under this task in the task hierarchy tree
        /// </summary>
        public List<Guid> ChildTaskIds { get; set; } = new List<Guid>();
        //[BsonIgnore] // TODO: uncomment fields if needed; delete later if not
        //public List<TaskModel> ChildTasks { get; set; }
    }
}
