using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GISApi.Data.GlobalEntities;

[Table("ControllerTimeSetting")]
public partial class ControllerTimeSetting
{
    [Key]
    public int Id { get; set; }

    [Column("CurrentDate_Server", TypeName = "datetime")]
    public DateTime? CurrentDateServer { get; set; }

    [Column("CuttentTime_Server_HH")]
    [StringLength(500)]
    public string? CuttentTimeServerHh { get; set; }

    [Column("CurrentDay_Server")]
    public int? CurrentDayServer { get; set; }

    [Column("DayStartTime_HH")]
    [StringLength(500)]
    public string? DayStartTimeHh { get; set; }

    [Column("DayEndTime_HH")]
    [StringLength(500)]
    public string? DayEndTimeHh { get; set; }

    [StringLength(500)]
    public string? UserId { get; set; }

    public int? ControllerId { get; set; }

    [StringLength(500)]
    public string? ControllerNo { get; set; }

    [Column("CurrentTime_Server_Min")]
    [StringLength(500)]
    public string? CurrentTimeServerMin { get; set; }

    [Column("DayStartTime_Min")]
    [StringLength(500)]
    public string? DayStartTimeMin { get; set; }

    [Column("DayEndTime_Min")]
    [StringLength(500)]
    public string? DayEndTimeMin { get; set; }

    [Column("Usermobile_IMEINo")]
    [StringLength(500)]
    public string? UsermobileImeino { get; set; }
}
