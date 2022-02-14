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
        [Authorize(Roles = "User")]
        public async Task<ActionResult> DeleteAsync(string[] selectedUsers) 
        {
            int i = 0;
            ApplicationUserManager userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var AuthenticationManager = HttpContext.GetOwinContext().Authentication;
            while (i < selectedUsers.Length)
            {
                if (selectedUsers[i] == "User" || selectedUsers[i] == "Blocked")
                {
                    await userManager.RemoveFromRoleAsync(selectedUsers[i + 1], selectedUsers[i]);
                    await userManager.AddToRoleAsync(selectedUsers[i + 1], "Blocked");
                    await userManager.SetLockoutEndDateAsync(selectedUsers[i + 1], DateTimeOffset.MaxValue);
                    i++;
                }
                else if (selectedUsers.Length - 2 < i)
                {
                    break;
                }
                else
                {
                    i++;
                }
                AuthenticationManager.SignOut();
            }
            return RedirectToAction("Login", "Account");
        }

        [HttpPost]
        [Authorize(Roles = "User")]
        public async Task<ActionResult> BlockAsync(string[] selectedUsers) 
        {
            int i = 0;
            ApplicationUserManager userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                while (i<selectedUsers.Length)
                {
                    if (selectedUsers[i] == "User" || selectedUsers[i] == "Deleted") {
                        await userManager.RemoveFromRoleAsync(selectedUsers[i + 1], selectedUsers[i]);
                        await userManager.AddToRoleAsync(selectedUsers[i + 1], "Blocked");
                        await userManager.SetLockoutEndDateAsync(selectedUsers[i + 1], DateTimeOffset.MaxValue);
                        i++;
                    }
                    else if(selectedUsers.Length - 2 < i)
                    {
                        break;
                    }
                    else
                    {
                        i++;
                    }
                }
            return RedirectToAction("Index","Home");
        }

        [HttpPost]
        [Authorize(Roles = "User")]
        public async Task<ActionResult> UnlockAsync(string[] selectedUsers)
        {
            int i = 0;
            ApplicationUserManager userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            while (i < selectedUsers.Length)
            {
                if (selectedUsers[i] == "Blocked")
                {
                    await userManager.RemoveFromRoleAsync(selectedUsers[i + 1], selectedUsers[i]);
                    await userManager.AddToRoleAsync(selectedUsers[i + 1], "User");
                    await userManager.SetLockoutEndDateAsync(selectedUsers[i + 1], DateTimeOffset.MaxValue);
                    i++;
                }
                else if (selectedUsers.Length - 2 < i)
                {
                    break;
                }
                else
                {
                    i++;
                }
            }
            return RedirectToAction("Index", "Home");
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