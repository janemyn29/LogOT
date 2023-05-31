using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mentor_v1.Domain.Entities;
public class DetailTaxIncome : BaseAuditableEntity
{
    public double Muc_chiu_thue { get; set; }
    public double Thue_suat { get; set; }
}
