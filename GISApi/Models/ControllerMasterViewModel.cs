using System.ComponentModel.DataAnnotations;

namespace GISApi.Models
{
    public class ControllerMasterViewModel
    {
        public int Id { get; set; }

        public string? ControllerNo { get; set; }

        public bool? IsActive { get; set; }

        public string? DateTime { get; set; }

    }
}
