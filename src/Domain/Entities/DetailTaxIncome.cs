using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogOT.Domain.Entities;
public class DetailTaxIncome : BaseAuditableEntity
{
    public string? Muc_chiu_thue { get; set; }
    public int? Thue_suat { get; set; }
}
