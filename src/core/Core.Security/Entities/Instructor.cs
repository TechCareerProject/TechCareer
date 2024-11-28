using Core.Persistence.Repositories.Entities;

namespace Core.Security.Entities;

public class Instructor : Entity<Guid>
{
    public string Name { get; set; }
    public string About { get; set; }
}
