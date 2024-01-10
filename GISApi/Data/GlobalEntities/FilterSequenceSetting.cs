using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GISApi.Data.GlobalEntities;

[Table("FilterSequenceSetting")]
public partial class FilterSequenceSetting
{
    [Key]
    public int Id { get; set; }

    [StringLength(500)]
    public string MaxFilterValve { get; set; } = null!;

    public int PumpNo { get; set; }

    [StringLength(500)]
    public string ValvePumpNo { get; set; } = null!;

    [Column("FlushTime_min")]
    [StringLength(500)]
    public string FlushTimeMin { get; set; } = null!;

    [Column("Interval_hh")]
    [StringLength(500)]
    public string IntervalHh { get; set; } = null!;

    [StringLength(500)]
    public string UserId { get; set; } = null!;

    [StringLength(500)]
    public string ControllerId { get; set; } = null!;

    [Column("FlushTime_sec")]
    [StringLength(500)]
    public string? FlushTimeSec { get; set; }

    [Column("Interval_min")]
    [StringLength(500)]
    public string? IntervalMin { get; set; }

    [Column("Interval_sec")]
    [StringLength(500)]
    public string? IntervalSec { get; set; }

    [Column("Usermobile_IMEINo")]
    [StringLength(500)]
    public string? UsermobileImeino { get; set; }
}
