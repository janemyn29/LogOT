using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using mentor_v1.Application.Common.Mappings;
using mentor_v1.Domain.Enums;

namespace mentor_v1.Application.Allowance.Queries.GetAllowance;
public class AllowanceViewModel : IMapFrom<Domain.Entities.Allowance>
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string AllowanceType { get; set; }
    public double Amount { get; set; }
    public string Eligibility_Criteria { get; set; }
    public string Requirements { get; set; }
}
