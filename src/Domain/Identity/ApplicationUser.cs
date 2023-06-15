using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace mentor_v1.Domain.Identity;

public class ApplicationUser : IdentityUser
{
    [ForeignKey("Position")]
    public Guid PositionId { get; set; }
    public string Fullname { get; set; }
    public GenderType GenderType { get; set; }
    public string Address { get; set; }
    public string Image { get; set; }
    public string IdentityNumber { get; set; }
    public DateTime BirthDay { get; set; }
    public string BankAccountNumber { get; set; }
    public string BankAccountName { get; set; }
    public string BankName { get; set; }
    public bool IsMaternity { get; set; } = false;
    public string? ImageBase { get;set; }
    public WorkStatus WorkStatus { get; set; }
    public IList<Experience> Experiences { get; private set; }
    public IList<OvertimeLog> OvertimeLogs { get; private set; }
    public IList<LeaveLog> LeaveLogs { get; private set; }

    public IList<EmployeeContract> EmployeeContracts { get; private set; }
    public IList<SkillEmployee> SkillEmployees { get; private set; }
    public IList<RequestChange> RequestChanges { get; private set; }
    public IList<Degree> Degrees { get; private set; }
    public IList<Attendance> Attendances { get; private set; }
    public IList<MaternityEmployee> MaternityEmployees { get; private set; }


    public virtual Position Position { get; set; }

}
