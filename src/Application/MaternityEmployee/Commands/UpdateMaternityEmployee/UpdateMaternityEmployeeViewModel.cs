﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mentor_v1.Application.Common.Mappings;

namespace mentor_v1.Application.MaternityEmployee.Commands.UpdateMaternityEmployee;
public class UpdateMaternityEmployeeViewModel : IMapFrom<Domain.Entities.MaternityEmployee>
{
    public Guid Id { get; set; }
    public string Image { get; set; }
    public DateTime BirthDay { get; set; }
    public int AcceptanceType { get; set; }
    public string DenyReason { get; set; }
}
