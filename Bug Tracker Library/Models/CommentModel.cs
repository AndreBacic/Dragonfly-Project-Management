using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace Bug_Tracker_Library.Models
{
    /// <summary>
    /// A comment posted by a User to a Project.
    /// </summary>
    [BsonIgnoreExtraElements]
    public class CommentModel
    {
        /// <summary>
        /// The id of the User who posted this.
        /// </summary>
        public Guid PosterId { get; set; }
        /// <summary>
        /// When this was posted.
        /// </summary>
        public DateTime DatePosted { get; set; }
        /// <summary>
        /// The actual text that the comment is.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// List of the IDs to navigate the project tree to this's parent project.
        /// </summary>
        public List<Guid> ParentProjectIdTreePath { get; set; } = new();
    }
}
