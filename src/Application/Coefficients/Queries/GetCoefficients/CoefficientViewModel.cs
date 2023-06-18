using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using mentor_v1.Application.Common.Mappings;
using mentor_v1.Domain.Entities;
using mentor_v1.Domain.Enums;

namespace mentor_v1.Application.Coefficients.Queries.GetCoefficients;
public class CoefficientViewModel : IMapFrom<Domain.Entities.Coefficient>
{
    public Guid Id { get; set; }
    public double AmountCoefficient { get; set; }
    public  string TypeDate { get; set; }
}
