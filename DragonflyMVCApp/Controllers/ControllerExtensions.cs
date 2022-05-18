using DragonflyDataLibrary;
using DragonflyDataLibrary.DataAccess;
using DragonflyDataLibrary.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
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


        public static async void LogInUser(this Controller @this, UserModel user, string role = UserRoles.USER)
        {
            List<Claim> personClaims = new()
            {
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.EmailAddress),
                new Claim(ClaimTypes.Role, role) // demo user has "Demo user" role
            };

            await @this.HttpContext.SignInAsync(new ClaimsPrincipal(
                new ClaimsIdentity(personClaims, "Dragonfly.Auth.Identity")));
        }

        public static bool IsValidEmailAddress(this Controller @this, string emailAddress) // TODO: move to an email logic class
        {
            try
            {
                System.Net.Mail.MailAddress m = new(emailAddress);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        public static string ClaimValue(this ClaimsPrincipal @this, UserClaimsIndex index)
        {
            return @this.Claims.ToList()[(int)index].Value;
        }
    }
}
