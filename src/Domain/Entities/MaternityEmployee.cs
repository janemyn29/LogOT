﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mentor_v1.Domain.Identity;

namespace mentor_v1.Domain.Entities;
public class MaternityEmployee : BaseAuditableEntity
{

    [ForeignKey("ApplicationUser")]
    public string ApplicationUserId { get; set; }
    [ForeignKey("MaternityAllowance")]
    public Guid MaternityAllowanceId { get; set; }

    public string Image { get; set; }
    public DateTime? DueDate { get; set; }
    public DateTime? BirthDay { get; set; }
    public AcceptanceType AcceptanceType { get; set; }
    public virtual ApplicationUser ApplicationUser { get; set; }
    public virtual MaternityAllowance MaternityAllowance { get; set; }
}
