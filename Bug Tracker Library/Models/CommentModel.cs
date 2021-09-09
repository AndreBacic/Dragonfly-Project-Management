using System;

namespace Bug_Tracker_Library.Models
{
    /// <summary>
    /// A comment posted by a User to a Project.
    /// </summary>
    public class CommentModel
    {
        /// <summary>
        /// The User who posted this.
        /// </summary>
        public virtual UserModel Poster { get; set; }
        /// <summary>
        /// Project this is posted to.
        /// </summary>
        public virtual ProjectModel Project { get; set; }
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
