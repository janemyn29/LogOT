using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mentor_v1.Application.Common.Mappings;
using mentor_v1.Application.ExcelContract.Queries.GetListExcelContacts;
using mentor_v1.Application.ExcelEmployeeQuit.Queries.GetListExcelEmployeeQuit;

namespace mentor_v1.Application.JobReport.Queries.GetJobReport;
public class GetJobReportByIdViewModel : IMapFrom<Domain.Entities.JobReport>
{
    public List<ExcelContractsViewModel>? ExcelContracts { get; init; }
    public List<ExcelEmployeeQuitViewModel>? ExcelEmployeeQuits { get; init; }
}
