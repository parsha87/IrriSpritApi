namespace GISApi.Models
{
    public class ValveSettingViewModel
    {
     
        public int Id { get; set; }

       
        public string MainValveNo { get; set; } = null!;

        public string TagName { get; set; } = null!;

        public string DurationHh { get; set; } = null!;

        public int PumpNo { get; set; }

        
        public string FbTimeHh { get; set; } = null!;

       
        public string FoTimeHh { get; set; } = null!;

       
        public string CoValveSetting { get; set; } = null!;

        
        public string CoValveNo1 { get; set; } = null!;

      
        public string CoValveNo2 { get; set; } = null!;

       
        public string CoValveNo3 { get; set; } = null!;

        public int UserId { get; set; }

        public int ControllerNo { get; set; }

       
        public string? DurationMm { get; set; }

        
        public string? DurationSs { get; set; }

        
        public string? FbTimeMin { get; set; }

        public string? FoTimeMin { get; set; }

      
        public string? CropName { get; set; }

       
        public string? CropType { get; set; }

      
        public DateTime? CropSowingDate { get; set; }

        public string? ValveArea { get; set; }

       
        public string? UsermobileImeino { get; set; }

    }
}
