using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mentor_v1.Domain.Entities;
public class MaternityAllowance : BaseEntity
{
    public string Descrition { get; set; }
    public double Amount { get; set; }
    public IList<MaternityEmployee> MaternityEmployees { get; set; }
}
