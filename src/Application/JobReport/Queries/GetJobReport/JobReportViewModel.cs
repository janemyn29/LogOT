using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mentor_v1.Application.Common.Mappings;
using mentor_v1.Application.ExcelContract.Queries.GetListExcelContacts;
using mentor_v1.Application.ExcelEmployeeQuit.Queries.GetListExcelEmployeeQuit;

namespace mentor_v1.Application.JobReport.Queries.GetJobReport;
public class JobReportViewModel : IMapFrom<Domain.Entities.JobReport>
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Job { get; set; }
    public DateTime ActionDate { get; set; }
    public string ActionType { get; set; }
}
