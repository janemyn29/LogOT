using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mentor_v1.Domain.Entities;
public class Exchange : BaseAuditableEntity
{
    public double? Muc_Quy_Doi_To { get; set; }
    public double Muc_Quy_Doi_From { get; set; }

    public double Giam_Tru { get; set; }
    public double Thue_Suat { get; set; }
}