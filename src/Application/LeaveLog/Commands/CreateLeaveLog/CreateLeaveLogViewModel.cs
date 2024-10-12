using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mentor_v1.Domain.Enums;

namespace mentor_v1.Application.LeaveLog.Commands.CreateLeaveLog;
public class CreateLeaveLogViewModel
{
    //public string ApplicationUserId { get; set; }
    public DateTime LeaveDate { get; set; }
    public LeaveShift LeaveShift { get; set; }
    public string Reason { get; set; }
}
