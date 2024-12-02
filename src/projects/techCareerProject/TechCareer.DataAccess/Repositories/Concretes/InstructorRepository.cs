using Core.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using TechCareer.DataAccess.Contexts;
using TechCareer.DataAccess.Repositories.Abstracts;
using TechCareer.Models.Entities;

namespace TechCareer.DataAccess.Repositories.Concretes;

public class InstructorRepository : EfRepositoryBase<Instructor, Guid, BaseDbContext>, IInstructorRepository
{
    public InstructorRepository(BaseDbContext context) : base(context)
    {
    }
}
