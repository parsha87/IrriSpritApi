using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GISApi.Data.GlobalEntities;

[Table("SequenceSetting")]
public partial class SequenceSetting
{
    [Key]
    public int Id { get; set; }

    [StringLength(500)]
    public string SequenceNo { get; set; } = null!;

    public int PumbNo { get; set; }

    [Column("TimeSlot1_HH")]
    [StringLength(500)]
    public string TimeSlot1Hh { get; set; } = null!;

    [Column("TimeSlot2_HH")]
    [StringLength(500)]
    public string TimeSlot2Hh { get; set; } = null!;

    [Column("TimeSlot3_HH")]
    [StringLength(500)]
    public string TimeSlot3Hh { get; set; } = null!;

    [Column("TimeSlot4_HH")]
    [StringLength(500)]
    public string TimeSlot4Hh { get; set; } = null!;

    [StringLength(500)]
    public string WeekdaysString { get; set; } = null!;

    [StringLength(500)]
    public string? ValveNos { get; set; }

    [Column("ValveDuration_readonly")]
    [StringLength(500)]
    public string? ValveDurationReadonly { get; set; }

    [Column("IS_Fert")]
    public bool IsFert { get; set; }

    [StringLength(500)]
    public string UserId { get; set; } = null!;

    [StringLength(500)]
    public string ControllerNo { get; set; } = null!;

    [Column("TimeSlot1_Min")]
    [StringLength(500)]
    public string? TimeSlot1Min { get; set; }

    [Column("TimeSlot2_Min")]
    [StringLength(500)]
    public string? TimeSlot2Min { get; set; }

    [Column("TimeSlot3_Min")]
    [StringLength(500)]
    public string? TimeSlot3Min { get; set; }

    [Column("TimeSlot4_Min")]
    [StringLength(500)]
    public string? TimeSlot4Min { get; set; }

    [Column("Usermobile_IMEINo")]
    [StringLength(500)]
    public string? UsermobileImeino { get; set; }

    public int? ControllerId { get; set; }
}
