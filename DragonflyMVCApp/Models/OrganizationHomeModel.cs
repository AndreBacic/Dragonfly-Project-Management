using DragonflyDataLibrary.Models;

namespace DragonflyMVCApp.Models
{
    public class OrganizationHomeModel
    {
        public OrganizationModel Organization { get; set; }
        public UserModel User { get; set; }
        public AssignmentModel UserAssignment { get; set; }
    }
}
