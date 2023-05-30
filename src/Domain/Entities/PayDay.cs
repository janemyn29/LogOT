using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mentor_v1.Domain.Entities;
public class PayDay : BaseAuditableEntity
{
    public DateTime PaymentDay { get; set; }
}