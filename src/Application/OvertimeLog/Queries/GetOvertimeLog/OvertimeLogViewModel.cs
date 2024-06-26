﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mentor_v1.Domain.Enums;

namespace mentor_v1.Application.OvertimeLog.Queries.GetOvertimeLog;
public class OvertimeLogViewModel
{
    [ForeignKey("ApplicationUser")]
    public string ApplicationUserId { get; set; }
    public DateTime Date { get; set; }
    public int Hours { get; set; }
    public string? CancelReason { get; set; }
    public LogStatus Status { get; set; }
    public virtual Domain.Identity.ApplicationUser ApplicationUser { get; set; }
}
