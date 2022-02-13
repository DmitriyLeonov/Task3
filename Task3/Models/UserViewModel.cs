using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Task3.Models
{
    public class UserViewModel
    {
        [Display(Name = "Id")]
        public string UserId { get; set; }
        [Display(Name = "User Name")]
        public string UserName { get; set; }
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Display(Name = "Status")]
        public string Role { get; set; }
        [Display(Name = "Registration date")]
        public DateTime RegistrationDate { get; set; }
        [Display(Name = "Last Login")]
        public DateTime LastLogIn { get; set; }
    }
}