using DragonflyDataLibrary.Models;
using DragonflyDataLibrary.Security;

namespace DragonflyDataLibrary
{
    public enum TaskStatus
    {
        TODO,
        IN_PROGRESS,
        COMPLETED,
        ARCHIVED
    }

    public enum TaskPriority
    {
        BOTTOM,
        LOW,
        MEDIUM,
        HIGH,
        TOP
    }

    public enum TaskType
    {
        Epic,
        Story,
        Test,
        Expense,
        Task,
        Bug
    }

    public enum ColorPreference
    {
        Light,
        Dark // default
    }

    public enum UserClaimsIndex // for the data in cookies
    {
        Name,
        Email,
        Role
    }

    public static class UserRoles
    {
        public const string DEMO_USER = "Demo User";
        public const string USER = "User";

        public static UserModel DemoUserModel
        {
            get
            {
                return new UserModel()
                {
                    FirstName = "John",
                    LastName = "Doe",
                    EmailAddress = "john12@demo.demonet",
                    PasswordHash = HashAndSalter.HashAndSalt("Secret123!").ToDbString(),
                    Projects = new System.Collections.Generic.List<ProjectModel>(),
                    ColorPreference = ColorPreference.Dark
                };
            }
        }
    }
}
