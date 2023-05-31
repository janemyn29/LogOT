﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mentor_v1.Domain.Entities;
public class Subsidize : BaseAuditableEntity
{
    public string Name { get; set; }
    public string Description { get; set; }
    public double Amount { get; set; }

    public IList<DepartmentAllowance> DepartmentAllowances { get; set; }
}