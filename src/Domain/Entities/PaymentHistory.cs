using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogOT.Domain.Entities;
public class PaymentHistory : BaseAuditableEntity
{

    [ForeignKey("CompanyContract")]
    public Guid CompanyContractId { get; set; }
    public double? Total { get; set; }
    public double? Tax { get; set; }
    public string? Note { get; set; }
    public double? TotalDeduction { get; set; }
    public DateTime? Date { get; set; }
    public double? TotalBonus { get; set; }
    public string? AcceptanceCode { get; set; }
    public PaymentHistoryStatus Status { get; set; }

    //relationship
    public virtual CompanyContract CompanyContract { get; set; } = null!;
}

