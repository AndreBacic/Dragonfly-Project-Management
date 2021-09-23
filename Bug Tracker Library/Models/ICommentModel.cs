using System;
using System.Collections.Generic;

namespace Bug_Tracker_Library.Models
{
    /// <summary>
    /// A comment posted by a User to a Project.
    /// </summary>
    public interface ICommentModel
    {
        /// <summary>
        /// The User who posted this.
        /// </summary>
        IUserModel Poster { get; set; }
        /// <summary>
        /// When this was posted.
        /// </summary>
        DateTime DatePosted { get; set; }
        /// <summary>
        /// The actual text that the comment is.
        /// </summary>
        string Text { get; set; }

        /// <summary>
        /// List of the IDs to navigate the project tree to this's parent project.
        /// </summary>
        List<Guid> ParentProjectIdTreePath { get; set; }
    }
}
