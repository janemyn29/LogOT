using mentor_v1.Application.Common.Mappings;
using mentor_v1.Domain.Enums;
using mentor_v1.Domain.Identity;

namespace WebUI.Models;

public class UserDisplayModel:IMapFrom<ApplicationUser>
{
    public Guid PositionId { get; set; }
    public string Fullname { get; set; }
    public string GenderType { get; set; }
    public string Address { get; set; }
    public string Image { get; set; }
    public string IdentityNumber { get; set; }
    public DateTime BirthDay { get; set; }
    public string BankAccountNumber { get; set; }
    public string BankAccountName { get; set; }
    public string BankName { get; set; }
    public bool IsMaternity { get; set; } = false;
    public string WorkStatus { get; set; }
}
