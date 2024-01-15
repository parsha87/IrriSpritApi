using GISApi.Data.GlobalEntities;
using System.ComponentModel.DataAnnotations;


namespace GISApi.Models
{

    public class AddEditUserViewModel
    {
        public string Id { get; set; }
        [Required]
        [EmailAddress]
        [MaxLength(255)]
        public string? UserName { get; set; }
        [StringLength(100, ErrorMessage = "UserId Max Length is 100")]

        public string? Email { get; set; }

        [MaxLength(30)]
        public string PhoneNumber { get; set; }

        [Required]
        [MaxLength(255)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(255)]
        public string LastName { get; set; }
        public string Address { get; set; }
        public string Password { get; set; }
        [Required]
        public bool IsActive { get; set; }
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public int CountryId { get; set; }
        public string CountryName { get; set; }
        public string TimeZone { get; set; }
        public int LanguageId { get; set; }
        public string LanguageName { get; set; }
        public string ParentId { get; set; }
        public bool IsParent { get; set; }
        public string SubUserEmail{ get; set; }
        public string SubUserPhoneNo { get; set; }
        public string SubUserFirstName { get; set; }
        public string SubUserLastName { get; set; }
        public List<UserControllerMapping> userControllerMappings { get; set; } = new List<UserControllerMapping>();
       

    }

    public class ManagersFroDDL
    {
        public string UserId { get; set; }
        public string Name { get; set; }
    }
}
