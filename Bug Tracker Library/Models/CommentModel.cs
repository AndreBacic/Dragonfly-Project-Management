using System;
using System.Collections.Generic;

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
        public virtual List<Guid> ParentProjectIdTreePath { get; set; } = new List<Guid>();
    }
}
