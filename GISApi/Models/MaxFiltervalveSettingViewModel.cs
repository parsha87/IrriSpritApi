using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GISApi.Models
{
    public class MaxFiltervalveSettingViewModel
    {
        
        public int Id { get; set; }

       
        public string MaxFilter { get; set; } = null!;

       
        public string TotalFilterValvePump1 { get; set; } = null!;

       
        public int FilterValveWithPump1 { get; set; }

        public string TotalFilterValvePump2 { get; set; } = null!;

       
        public int FilterValveWithPump2 { get; set; }

        public int MaxValve { get; set; }

        public string IrrigateValve { get; set; } = null!;

       
        public string UserId { get; set; } = null!;

        public string ControllerNo { get; set; } = null!;

       
        public string? UsermobileImeino { get; set; }
    }
}
