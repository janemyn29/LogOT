using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogOT.Domain.Entities;
public class PaymentDay : BaseAuditableEntity
{
    public DateTime PayDay { get; set; }
}