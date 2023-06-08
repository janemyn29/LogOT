using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using mentor_v1.Application.Common.Interfaces;
using mentor_v1.Application.Common.Mappings;
using mentor_v1.Application.Common.Models;
using mentor_v1.Application.Level.Queries.GetLevel;
using mentor_v1.Domain.Entities;

namespace mentor_v1.Application.Department.Queries.GetDepartment;

public class DepartmentViewModel : IMapFrom<Domain.Entities.Department>
{

    public Guid Id { get; init; }
    public string Name { get; set; }

    public string Description { get; set; }

    public IList<Position> Positions { get; set; }

}