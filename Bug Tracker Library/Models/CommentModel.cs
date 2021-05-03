using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bug_Tracker_Library.Models
{
    /// <summary>
    /// A comment posted by a User to a Project.
    /// </summary>
    public class CommentModel
    {
        public int Id { get; set; }
        /// <summary>
        /// The User who posted this.
        /// </summary>
        public UserModel Poster { get; set; }
        /// <summary>
        /// Project this is posted to.
        /// </summary>
        public ProjectModel Project { get; set; }
        /// <summary>
        /// When this was posted.
        /// </summary>
        public DateTime DatePosted { get; set; }
        /// <summary>
        /// The actual text that the comment is.
        /// </summary>
        public string Text { get; set; }
    }
}
