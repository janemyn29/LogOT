using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mentor_v1.Domain.Entities;
public class DetailTax : BaseAuditableEntity
{
    [ForeignKey("Payslip")]
    public Guid PayslipId { get; set; }
    public double Muc_chiu_thue_From { get; set; }
    public double? Muc_chiu_thue_To { get; set; }
    public double Thue_suat { get; set; }

    public double TaxAmount { get; set; }

   public virtual PaySlip PaySlip { get; set; }
}
