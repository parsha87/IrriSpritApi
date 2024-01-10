using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GISApi.Data.GlobalEntities;

[Table("ControllerMaster")]
public partial class ControllerMaster
{
    [Key]
    public int Id { get; set; }

    [StringLength(100)]
    public string? ControllerNo { get; set; }

    public bool? IsActive { get; set; }
}
