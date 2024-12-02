using Core.Security.Entities;
using TechCareer.Service.Abstracts;

namespace TechCareer.Service.Concretes;

public class InstructorService : IInstructorService
{
    private readonly IInstructorRepository _instructorRepository;

    public InstructorService(IInstructorRepository instructorRepository)
    {
        _instructorRepository = instructorRepository;
    }

    public async Task<IEnumerable<Instructor>> GetAllAsync()
    {
        return await _instructorRepository.GetAllAsync();
    }

    public async Task<Instructor> GetByIdAsync(int instructorId)
    {
        return await _instructorRepository.GetByIdAsync(instructorId);
    }

    public async Task AddAsync(Instructor instructor)
    {
        await _instructorRepository.AddAsync(instructor);
    }

    public async Task UpdateAsync(Instructor instructor)
    {
        await _instructorRepository.UpdateAsync(instructor);
    }

    public async Task DeleteAsync(int instructorId)
    {
        await _instructorRepository.DeleteAsync(instructorId);
    }
}
