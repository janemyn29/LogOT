using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mentor_v1.Domain.Enums;
public enum ShiftType
{
    Full =1,
    MorningAndAfternoon = 2,
    Morning = 3,
    Afternoon = 4,
    Night = 5,
    AfternoonAndNight = 6,
    MorningAndNight= 7,
    NotWork = 8,
}
