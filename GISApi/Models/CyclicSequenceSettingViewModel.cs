namespace GISApi.Models
{
    public class CyclicSequenceSettingViewModel
    {
       
        public int Id { get; set; }

        public string SequenceNo { get; set; } = null!;

        public int PumpNo { get; set; }


        public string StartTimeHh { get; set; } = null!;

      
        public string EndTimeHh { get; set; } = null!;

        public string Interval { get; set; } = null!;

        public string WeekdaysString { get; set; } = null!;

        public string ValveNo { get; set; } = null!;

        public string ValveDuration { get; set; } = null!;

        public string UserId { get; set; } = null!;

        public string ControllerNo { get; set; } = null!;

        public string? StartTimeMin { get; set; }

        public string? EndTimeMin { get; set; }

        public string? UsermobileImeino { get; set; }
    }
}
