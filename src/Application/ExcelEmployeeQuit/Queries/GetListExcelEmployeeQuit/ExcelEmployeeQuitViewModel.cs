using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mentor_v1.Application.Common.Mappings;
using mentor_v1.Domain.Enums;

namespace mentor_v1.Application.ExcelEmployeeQuit.Queries.GetListExcelEmployeeQuit;
public class ExcelEmployeeQuitViewModel : IMapFrom<Domain.Entities.ExcelEmployeeQuit>
{
    public Guid JobReportId { get; set; }
    public string FullName { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string WorkStatus { get; set; }
    public string ActionType { get; set; }
    public DateTime ActionDate { get; set; }
}
