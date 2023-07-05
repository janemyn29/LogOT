using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mentor_v1.Domain.Entities;
public class JobReport :BaseAuditableEntity
{
    public string Title { get; set; }
    public string Job { get; set; }
    public DateTime ActionDate { get; set; }

    public ActionType ActionType { get; set; }

    public IList<ExcelContract> ExcelContracts { get; init; }
    public IList<ExcelEmployeeQuit> ExcelEmployeeQuits { get; init; }


}
