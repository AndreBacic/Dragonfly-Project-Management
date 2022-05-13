using DragonflyDataLibrary;
using DragonflyDataLibrary.DataAccess;
using DragonflyDataLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;

namespace DragonflyMVCApp.Controllers
{
    public static class ControllerExtensions
    {
        public static UserModel GetLoggedInUserByEmail(this Controller @this, IDataAccessor _db)
        {
            string email = @this.User.ClaimValue(UserClaimsIndex.Email);
            return _db.GetUser(email);
        }

        public static string ClaimValue(this ClaimsPrincipal @this, UserClaimsIndex index)
        {
            return @this.Claims.ToList()[(int)index].Value;
        }
    }
}
