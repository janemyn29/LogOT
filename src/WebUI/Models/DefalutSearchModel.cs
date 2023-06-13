using mentor_v1.Domain.Identity;

namespace WebUI.Models;

public class DefalutSearchModel<T> where T : class
{
    public string? Keyword { get; set; }
    public T DefautList { get; set; }
}
