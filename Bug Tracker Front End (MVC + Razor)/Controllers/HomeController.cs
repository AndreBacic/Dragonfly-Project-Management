using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Bug_Tracker_Front_End__MVC_plus_Razor.Models;
using Bug_Tracker_Library.Models;
using Bug_Tracker_Library.DataAccess;

namespace Bug_Tracker_Front_End__MVC_plus_Razor.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IDataAccessor _dataAccessor;

        public HomeController(ILogger<HomeController> logger, IDataAccessor dataAccessor)
        {
            _logger = logger;
            _dataAccessor = dataAccessor;
        }

        // GET: Index (user login page)
        public IActionResult Index()
        {
            return View();
        }
        // POST: Home/Index (validates login) TODO: Learn if this is a POST protocol or some other one.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(string username, string password)
        {
            try
            {
                // TODO: validate user login. redirect to organization home if correct username/password, redirect back to Index or to LoginFailed if incorrect.

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public IActionResult CreateAccount()
        {
            return View();
        }
        // POST: Home/CreateAccount
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateAccount(CreateUserViewModel model)
        {
            try
            {
                // TODO: Validate that user data is good. Probably should use Regex.

                // TODO: Hash the password before storing it.
                string hashedPassword = model.Password;

                UserModel newUser = new UserModel
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    EmailAddress = model.EmailAddress,
                    PhoneNumber = model.PhoneNumber,
                    PasswordHash = hashedPassword
                };

                if (_dataAccessor.CreateUser(newUser))
                {
                    return RedirectToAction("OrganizationHome", "Organization");
                }
                else
                {
                    return View();
                }
            }
            catch
            {
                return View();
            }
        }
        public IActionResult LoginFailed()
        {
            return View();
        }

        /// <summary>
        /// Tells user that he/she has been successfully logged out.
        /// </summary>
        /// <returns></returns>
        public IActionResult LoggedOut()
        {
            return View();
        }

        /// <summary>
        /// Allows user to login to an existing org or link to create a new one
        /// </summary>
        /// <returns></returns>
        public IActionResult OrganizationLogin()
        {
            return View();
        }

        /// <summary>
        /// This just handles the error message page
        /// </summary>
        /// <returns>A web page displaying an error message</returns>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
