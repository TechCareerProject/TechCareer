using Core.CrossCuttingConcerns.Exceptions.ExceptionTypes;
using Core.CrossCuttingConcerns.Rules;
using Core.Security.Entities;
using Core.Security.Hashing;
using TechCareer.DataAccess.Repositories.Abstracts;
using TechCareer.Service.Constants;

namespace TechCareer.Service.Rules;

public sealed class UserBusinessRules(IUserRepository _userRepository) : BaseBusinessRules
{
    public Task UserShouldBeExistsWhenSelected(User? user)
    {
        if (user == null)
            throw new BusinessException(AuthMessages.VideoEducationDontExists);
        return Task.CompletedTask;
    }

    public async Task UserIdShouldBeExistsWhenSelected(int id)
    {
        bool doesExist = await _userRepository.AnyAsync(predicate: u => u.Id == id, enableTracking: false);
        if (doesExist)
            throw new BusinessException(AuthMessages.VideoEducationDontExists);
    }

    public Task UserPasswordShouldBeMatched(User user, string password)
    {
        if (!HashingHelper.VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
            throw new BusinessException(AuthMessages.PasswordDontMatch);
        return Task.CompletedTask;
    }

    public async Task UserEmailShouldNotExistsWhenInsert(string email)
    {
        bool doesExists = await _userRepository.AnyAsync(predicate: u => u.Email == email, enableTracking: false);
        if (doesExists)
            throw new BusinessException(AuthMessages.UserMailAlreadyExists);
    }

    public async Task UserEmailShouldNotExistsWhenUpdate(int id, string email)
    {
        bool doesExists = await _userRepository.AnyAsync(predicate: u => u.Id != id && u.Email == email, enableTracking: false);
        if (doesExists)
            throw new BusinessException(AuthMessages.UserMailAlreadyExists);
    }
}