using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mentor_v1.Domain.Enums;

namespace mentor_v1.Application.OvertimeLog.Commands.UpdateOvertimeLog;
public class UpdateOvertimeLogViewModel
{
    //public string ApplicationUserId { get; set; }
    public DateTime Date { get; set; }
    public int Hours { get; set; }
}
