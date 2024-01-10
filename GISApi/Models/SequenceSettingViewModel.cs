namespace GISApi.Models
{
    public class SequenceSettingViewModel
    {
        
        public int Id { get; set; }

      
        public string SequenceNo { get; set; } = null!;

        public int PumbNo { get; set; }

       
        public string TimeSlot1Hh { get; set; } = null!;

      
        public string TimeSlot2Hh { get; set; } = null!;

      
        public string TimeSlot3Hh { get; set; } = null!;

       
        public string TimeSlot4Hh { get; set; } = null!;

        
        public string WeekdaysString { get; set; } = null!;

      
        public string? ValveNos { get; set; }

      
        public string? ValveDurationReadonly { get; set; }

    
        public bool IsFert { get; set; }

      
        public string UserId { get; set; } = null!;

       
        public string ControllerNo { get; set; } = null!;

       
        public string? TimeSlot1Min { get; set; }

       
        public string? TimeSlot2Min { get; set; }

        
        public string? TimeSlot3Min { get; set; }

       
        public string? TimeSlot4Min { get; set; }

      
        public string? UsermobileImeino { get; set; }
    }
}
