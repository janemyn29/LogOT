﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mentor_v1.Domain.Enums;

namespace mentor_v1.Application.LeaveLog.Queries.GetLeaveLog;
public class LeaveLogViewModel
{
    //[ForeignKey("ApplicationUser")]
    public string ApplicationUserId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int LeaveHours { get; set; }
    public string Reason { get; set; }
    public string? CancelReason { get; set; }
    public LogStatus Status { get; set; }
    //public virtual ApplicationUser ApplicationUser { get; set; }
}