﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GISApi.Data.GlobalEntities;

[Table("UserControllerMapping")]
public partial class UserControllerMapping
{
    [Key]
    public int Id { get; set; }

    [StringLength(500)]
    public string? UserId { get; set; }

    public int? ControllerId { get; set; }
}
