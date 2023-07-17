using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mentor_v1.Application.Common.Mappings;

namespace mentor_v1.Application.ExcelContract.Queries.GetListExcelContacts;
public class ExcelContractsViewModel : IMapFrom<Domain.Entities.ExcelContract>
{
    public Guid Id { get; set; }
    public Guid JobReportId { get; set; }
    public string ContractCode { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get;set; }
    public string EmployeeName { get; set; }
    public string ContractStatus { get; set; }
    public string Action { get; set; }
    public DateTime ActionDate { get; set; }
    public string Job { get; set; }

}
