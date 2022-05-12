using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace DragonflyDataLibrary.Models
{
    /// <summary>
    /// A Project to be tracked and managed by the appliction.
    /// </summary>
    [BsonIgnoreExtraElements]
    public class ProjectModel
    {
        /// <summary>
        /// Unique identifier for the project. // TODO: Figure out how to do this manually? Use index?
        /// </summary>
        public int Id { get; set; }
        public string Title { get; set; }

        /// <summary>
        /// The description of this, which may be just a short statement or a very long list of instructions to Workers.
        /// </summary>
        public string Description { get; set; }
        public DateTime Created { get; set; }
        /// <summary>
        /// The date that all work on this must be done.
        /// </summary>
        public DateTime Deadline { get; set; }
        public ProjectStatus Status { get; set; }
        public ProjectPriority Priority { get; set; }
    }
}
