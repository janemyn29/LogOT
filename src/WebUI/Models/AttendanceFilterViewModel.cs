using mentor_v1.Application.Attendance.Queries.GetAttendance;
using mentor_v1.Application.Common.PagingUser;

namespace WebUI.Models;

public class AttendanceFilterViewModel
{
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }

    public PagingAppUser<AttendanceViewModel>  list { get; set; }
}
