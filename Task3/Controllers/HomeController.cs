using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Task3.Models;

namespace Task3.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Users()
        {
            ApplicationDbContext context = new ApplicationDbContext();

            //IdentityRole identityRole = new IdentityRole();
            //ApplicationUserManager userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            //IList<UserViewModel> users = new List<UserViewModel>();
            //IList<ApplicationUser> allUsers = context.Users.ToList();
            //foreach (ApplicationUser user in allUsers)
            //{

            //    users.Add(new UserViewModel { User = user, Role = role });
            //}
            var usersWithRoles = (from user in context.Users
                                  select new
                                  {
                                      UserId = user.Id,
                                      Username = user.UserName,
                                      Email = user.Email,
                                      LastLogIn = user.LastLogIn,
                                      RegistrationDate = user.RegistrationDate,
                                      RoleNames = (from userRole in user.Roles
                                                   join role in context.Roles on userRole.RoleId
                                                   equals role.Id
                                                   select role.Name).ToList()
                                  }).ToList().Select(p => new UserViewModel()

                                  {
                                      UserId = p.UserId,
                                      UserName = p.Username,
                                      Email = p.Email,
                                      LastLogIn = p.LastLogIn,
                                      RegistrationDate = p.RegistrationDate,
                                      Role = string.Join(",", p.RoleNames)
                                  });
            return View(usersWithRoles);
        }
    }
}