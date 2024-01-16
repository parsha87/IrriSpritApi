using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GISApi.Data.GlobalEntities;

[Table("ValveSetting")]
public partial class ValveSetting
{
    [Key]
    public int Id { get; set; }

    [StringLength(500)]
    public string MainValveNo { get; set; } = null!;

    [StringLength(500)]
    public string TagName { get; set; } = null!;

    [Column("Duration_HH")]
    [StringLength(500)]
    public string DurationHh { get; set; } = null!;

    public int PumpNo { get; set; }

    [Column("FB_Time_HH")]
    [StringLength(500)]
    public string FbTimeHh { get; set; } = null!;

    [Column("FO_TimeHH")]
    [StringLength(500)]
    public string FoTimeHh { get; set; } = null!;

    [StringLength(500)]
    public string CoValveSetting { get; set; } = null!;

    [Column("Co_ValveNo1")]
    [StringLength(500)]
    public string CoValveNo1 { get; set; } = null!;

    [Column("Co_ValveNo2")]
    [StringLength(500)]
    public string CoValveNo2 { get; set; } = null!;

    [Column("Co_ValveNo3")]
    [StringLength(500)]
    public string CoValveNo3 { get; set; } = null!;

    public string UserId { get; set; }

    public string ControllerNo { get; set; }

    [Column("Duration_MM")]
    [StringLength(500)]
    public string? DurationMm { get; set; }

    [Column("Duration_SS")]
    [StringLength(500)]
    public string? DurationSs { get; set; }

    [Column("FB_Time_Min")]
    [StringLength(500)]
    public string? FbTimeMin { get; set; }

    [Column("FO_Time_Min")]
    [StringLength(500)]
    public string? FoTimeMin { get; set; }

    [Column("Crop_Name")]
    [StringLength(500)]
    public string? CropName { get; set; }

    [Column("Crop_Type")]
    [StringLength(500)]
    public string? CropType { get; set; }

    [Column("Crop_Sowing_Date", TypeName = "datetime")]
    public DateTime? CropSowingDate { get; set; }

    [Column("Valve_Area")]
    [StringLength(500)]
    public string? ValveArea { get; set; }

    [Column("Usermobile_IMEINo")]
    [StringLength(500)]
    public string? UsermobileImeino { get; set; }

    public int? ControllerId { get; set; }
}
