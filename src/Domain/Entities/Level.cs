﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mentor_v1.Domain.Identity;

namespace mentor_v1.Domain.Entities;
public class Level : BaseAuditableEntity
{
    [ForeignKey("Department")]
    public Guid DepartmentId { get; set; }

    public string Name { get; set; }

    public string  Description { get; set; }

    public virtual Department Department { get; set; }
}