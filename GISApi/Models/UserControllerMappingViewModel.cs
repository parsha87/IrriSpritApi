using System.ComponentModel.DataAnnotations;

namespace GISApi.Models
{
    public class UserControllerMappingViewModel
    {
        public int Id { get; set; }

      
        public string? UserId { get; set; }

        public int? ControllerId { get; set; }

    }
}
