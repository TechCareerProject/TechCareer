using Core.Security.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechCareer.Service.Abstracts
{
    public interface IInstructorService
    {
        Task<IEnumerable<Instructor>> GetAllAsync();
        Task<Instructor> GetByIdAsync(Guid id);
        Task AddAsync(Instructor instructor);
        Task UpdateAsync(Instructor instructor);
        Task DeleteAsync(Guid instructorId);

    }
}
