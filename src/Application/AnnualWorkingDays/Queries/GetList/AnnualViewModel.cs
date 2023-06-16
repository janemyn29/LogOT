using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mentor_v1.Application.Common.Mappings;
using mentor_v1.Domain.Entities;
using mentor_v1.Domain.Enums;

namespace mentor_v1.Application.AnnualWorkingDays.Queries.GetList;
public class AnnualViewModel: IMapFrom<Domain.Entities.AnnualWorkingDay>
{
    public Guid Id { get; set; }
    public Guid CoefficientId { get; set; }
    public DateTime Day { get; set; }
    public string ShiftType { get; set; }
    public string TypeDate { get; set; }

    public virtual Coefficient Coefficient { get; set; }
}
