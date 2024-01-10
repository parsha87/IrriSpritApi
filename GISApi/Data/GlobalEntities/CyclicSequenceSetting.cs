using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GISApi.Data.GlobalEntities;

[Table("CyclicSequenceSetting")]
public partial class CyclicSequenceSetting
{
    [Key]
    public int Id { get; set; }

    [StringLength(500)]
    public string SequenceNo { get; set; } = null!;

    public int PumpNo { get; set; }

    [Column("StartTime_HH")]
    [StringLength(500)]
    public string StartTimeHh { get; set; } = null!;

    [Column("EndTime_HH")]
    [StringLength(500)]
    public string EndTimeHh { get; set; } = null!;

    [StringLength(500)]
    public string Interval { get; set; } = null!;

    [StringLength(500)]
    public string WeekdaysString { get; set; } = null!;

    [StringLength(500)]
    public string ValveNo { get; set; } = null!;

    [StringLength(500)]
    public string ValveDuration { get; set; } = null!;

    [StringLength(500)]
    public string UserId { get; set; } = null!;

    [StringLength(500)]
    public string ControllerNo { get; set; } = null!;

    [Column("StartTime_Min")]
    [StringLength(500)]
    public string? StartTimeMin { get; set; }

    [Column("EndTime_Min")]
    [StringLength(500)]
    public string? EndTimeMin { get; set; }

    [Column("Usermobile_IMEINo")]
    [StringLength(500)]
    public string? UsermobileImeino { get; set; }
}
