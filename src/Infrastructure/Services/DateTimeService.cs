using mentor_v1.Application.Common.Interfaces;

namespace mentor_v1.Infrastructure.Services;

public class DateTimeService : IDateTime
{
    public DateTime Now => DateTime.Now;
}
