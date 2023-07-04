using mentor_v1.Domain.Identity;

namespace WebUI.Services.AttendanceServices;

public interface IAttendanceService
{
    Task<string> AttendanceFullDay(DateTime now,ApplicationUser user);
    //Task<string> AttendanceDay(DateTime now, ApplicationUser user);
    Task<string> AttendanceMorningOnly(DateTime now, ApplicationUser user);
    Task<string> AttendanceAfternoonOnly(DateTime now, ApplicationUser user);
    Task<string> AttendanceNotWork(DateTime now, ApplicationUser user);

}
