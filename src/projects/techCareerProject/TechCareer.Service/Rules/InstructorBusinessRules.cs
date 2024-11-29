using Core.CrossCuttingConcerns.Exceptions.ExceptionTypes;
using Core.CrossCuttingConcerns.Rules;
using Core.Security.Entities;
using TechCareer.DataAccess.Repositories.Abstracts;
using TechCareer.Service.Constants;
using TechCareer.Service.Models;

namespace TechCareer.Service.Rules;

public sealed class InstructorBusinessRules(IInstructorRepository _instructorRepository) : BaseBusinessRules
{
    public Task InstructorShouldExistWhenSelected(Instructor? instructor)
    {
        if (instructor == null)
            throw new BusinessException(InstructorMessages.InstructorNotFound);
        return Task.CompletedTask;
    }

    public async Task InstructorIdShouldExistWhenSelected(Guid id)
    {
        bool doesExist = await _instructorRepository.AnyAsync(predicate: i => i.Id == id, enableTracking: false);
        if (!doesExist)
            throw new BusinessException(InstructorMessages.InstructorNotFound);
    }

    public async Task InstructorEmailShouldNotExistWhenInsert(string email)
    {
        bool doesExist = await _instructorRepository.AnyAsync(predicate: i => i.Email == email, enableTracking: false);
        if (doesExist)
            throw new BusinessException(InstructorMessages.EmailAlreadyExists);
    }

    public async Task InstructorEmailShouldNotExistWhenUpdate(Guid id, string email)
    {
        bool doesExist = await _instructorRepository.AnyAsync(predicate: i => i.Id != id && i.Email == email, enableTracking: false);
        if (doesExist)
            throw new BusinessException(InstructorMessages.EmailAlreadyExists);
    }
}