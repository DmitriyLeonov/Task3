using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Task3.Attributes;
using Task3.Models;

namespace Task3.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        //public ActionResult Delete() => View();

        [HttpPost]
        //[MultipleButton(Name = "action", Argument = "Delete")]
        public async Task<ActionResult> DeleteAsync(string[] selectedUsers) 
        {
            ApplicationUserManager userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            for (int i = 0; i < selectedUsers.Length; i+=2)
            {
                if (selectedUsers[i] == "User" || selectedUsers[i] == "Deleted" || selectedUsers[i] == "Blocked") { }
                else { 
                    i++;
                }
                if (selectedUsers.Length - 2 < i)
                {
                    break;
                }
                await userManager.RemoveFromRoleAsync(selectedUsers[i + 1], selectedUsers[i]);
                await userManager.AddToRoleAsync(selectedUsers[i + 1], "Deleted");
            }
            return View();
        }

        [HttpPost]
        //[MultipleButton(Name = "action", Argument = "Block")]
        public async Task<ActionResult> BlockAsync(string[] selectedUsers) 
        {
            ApplicationUserManager userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            for (int i = 0; i < selectedUsers.Length; i += 2)
            {
                if (selectedUsers[i] == "User" || selectedUsers[i] == "Deleted" || selectedUsers[i] == "Blocked") { }
                else
                {
                    i++;
                }
                if (selectedUsers.Length - 2 < i)
                {
                    break;
                }
                await userManager.RemoveFromRoleAsync(selectedUsers[i + 1], selectedUsers[i]);
                await userManager.AddToRoleAsync(selectedUsers[i + 1], "Blocked");
            }
            return View(AccountController.);
        }

        public ActionResult Users()
        {
            ApplicationDbContext context = new ApplicationDbContext();

            
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