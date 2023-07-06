using mentor_v1.Domain.Entities;
using mentor_v1.Domain.Identity;

namespace WebUI.Models;

public class PositionModel
{
    public ApplicationUser User { get; set; }
    public Position Position { get; set; }
    public List<Subsidize> Subsidize { get; set; }
}
