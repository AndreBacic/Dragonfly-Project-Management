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
        /// Unique identifier for the project.
        /// </summary>
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; } = "";

        /// <summary>
        /// Short description of the project, typically less than 100 characters.
        /// </summary> // TODO: Have view models restrict this to 100ish characters
        public string Description { get; set; } = "";
        /// <summary>
        /// Long description of project and any additional information the user wants to add.
        /// </summary>
        public string Notes { get; set; } = "";
        public DateTime Created { get; set; }
        /// <summary>
        /// The date that all work on this must be done.
        /// </summary>
        public DateTime Deadline { get; set; }
        public decimal Budget { get; set; }
        public List<TaskModel> Tasks { get; set; } = new();
    }
}
