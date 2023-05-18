﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogOT.Domain.IdentityModel;

namespace LogOT.Domain.Entities;
public class Experience : BaseAuditableEntity
{
    [ForeignKey("ApplicationUser")]
    public string UserId { get; set; }
    public string NameProject { get; set; }
    public int TeamSize { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Description { get; set; }
    public string TechStack { get; set; }

    public virtual ApplicationUser ApplicationUser { get; set; }




}
