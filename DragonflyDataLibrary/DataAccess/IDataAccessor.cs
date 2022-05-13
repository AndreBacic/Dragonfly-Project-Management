using DragonflyDataLibrary.Models;

namespace DragonflyDataLibrary.DataAccess
{
    /// <summary>
    /// Interface for a data access class
    /// <br/>
    /// NOTE: implementations should have a constructor with parameter (IConfiguration configuration)
    /// where configuration is for grabbing a connection string
    /// </summary>
    public interface IDataAccessor
    {
        UserModel GetUser(string emailAddress);
        void CreateUser(UserModel user);
    }
}
