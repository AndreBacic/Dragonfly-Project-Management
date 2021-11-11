using System;

namespace DragonflyMVCApp.Models
{
    public class CreateOrganizationModel
    {
        public string Name { get; set; }
        /// <summary>
        /// The ids of the users invited and assigned to work on this including the admin.
        /// </summary>
        public Guid CreatorId { get; set; }

        public string Description { get; set; }
    }
}
