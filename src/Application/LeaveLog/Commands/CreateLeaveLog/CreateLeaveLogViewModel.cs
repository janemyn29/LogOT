using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mentor_v1.Application.LeaveLog.Commands.CreateLeaveLog;
public class CreateLeaveLogViewModel
{
    public string ApplicationUserId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int LeaveHours { get; set; }
    public string Reason { get; set; }
}
