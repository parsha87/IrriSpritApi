using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GISApi.Data.GlobalEntities;

[Table("MaxFiltervalveSetting")]
public partial class MaxFiltervalveSetting
{
    [Key]
    public int Id { get; set; }

    [StringLength(500)]
    public string MaxFilter { get; set; } = null!;

    [Column("Total_Filter_Valve_Pump1")]
    [StringLength(500)]
    public string TotalFilterValvePump1 { get; set; } = null!;

    [Column("Filter_Valve_With_Pump1")]
    public int FilterValveWithPump1 { get; set; }

    [Column("Total_Filter_Valve_Pump2")]
    [StringLength(500)]
    public string TotalFilterValvePump2 { get; set; } = null!;

    [Column("Filter_Valve_With_Pump2")]
    public int FilterValveWithPump2 { get; set; }

    public int MaxValve { get; set; }

    [StringLength(500)]
    public string IrrigateValve { get; set; } = null!;

    [StringLength(500)]
    public string UserId { get; set; } = null!;

    [StringLength(500)]
    public string ControllerNo { get; set; } = null!;

    [Column("Usermobile_IMEINo")]
    [StringLength(500)]
    public string? UsermobileImeino { get; set; }
}
