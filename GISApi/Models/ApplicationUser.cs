using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GISApi.Models
{
    public class ApplicationUser : IdentityUser
    {
        [MaxLength(100)]
        public string FirstName { get; set; }
        [MaxLength(100)]
        public string LastName { get; set; }
        public bool IsActive { get; set; }
        public string RoleId { get; set; }
        public string Address { get; set; }       
        public string RoleName { get; set; }       
        public string? DisplayUserName { get; set; }



    }
}
