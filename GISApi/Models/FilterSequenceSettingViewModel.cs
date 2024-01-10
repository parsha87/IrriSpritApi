using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GISApi.Models
{
    public class FilterSequenceSettingViewModel
    {
       
        public int Id { get; set; }

       
        public string MaxFilterValve { get; set; } = null!;

        public int PumpNo { get; set; }

       
        public string ValvePumpNo { get; set; } = null!;

        
        public string FlushTimeMin { get; set; } = null!;

       
        public string IntervalHh { get; set; } = null!;

       
        public string UserId { get; set; } = null!;

       
        public string ControllerId { get; set; } = null!;

        
        public string? FlushTimeSec { get; set; }

      
        public string? IntervalMin { get; set; }

        
        public string? IntervalSec { get; set; }

        
        public string? UsermobileImeino { get; set; }
    }
}
