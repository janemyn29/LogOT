using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mentor_v1.Application.Common.Mappings;
using mentor_v1.Domain.Entities;
using mentor_v1.Domain.Enums;
using mentor_v1.Domain.Identity;

namespace mentor_v1.Application.Employee.Queries.GetEmployee;
public class EmployeeViewModel: IMapFrom<Domain.Entities.Employee>
{
    public string ApplicationUserId { get; set; }
    public string IdentityNumber { get; set; }
    public DateTime BirthDay { get; set; }
    public string BankAccountNumber { get; set; }
    public string BankAccountName { get; set; }
    public string BankName { get; set; }

    public DefaultStatus Status { get; set; }

    public IList<Experience> Experiences { get; private set; }
    public IList<OvertimeLog> OvertimeLogs { get; private set; }
    public IList<LeaveLog> LeaveLogs { get; private set; }

    public IList<EmployeeContract> EmployeeContracts { get; private set; }
    public IList<InterviewProcess> InterviewProcesses { get; private set; }
    public IList<SkillEmployee> SkillEmployees { get; private set; }

    public IList<RequestChange> RequestChanges { get; private set; }

    public virtual ApplicationUser ApplicationUser { get; set; }
}
