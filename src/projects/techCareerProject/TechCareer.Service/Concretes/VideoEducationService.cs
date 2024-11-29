using AutoMapper;
using Core.CrossCuttingConcerns.DtoBases;
using Core.Persistence.Extensions;
using Core.Security.Entities;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;
using TechCareer.DataAccess.Repositories.Abstracts;
using TechCareer.DataAccess.Repositories.Concretes;
using TechCareer.Models.Dtos.VideoEducation;
using TechCareer.Models.Entities;
using TechCareer.Service.Abstracts;
using TechCareer.Service.Constants;
using TechCareer.Service.Rules;

namespace TechCareer.Service.Concretes;

public sealed class VideoEducationService : IVideoEducationService
{
    private readonly IVideoEducationRepository _videoEducationRepository;
    private readonly IMapper _mapper;
    private readonly VideoEducationBusinessRules _businessRules;

    public VideoEducationService(IVideoEducationRepository videoEducationRepository, IMapper mapper, VideoEducationBusinessRules businessRules)
    {
        _videoEducationRepository = videoEducationRepository;
        _mapper = mapper;
        _businessRules = businessRules;
    }

    //[LoggerAspect]
    //[ClearCacheAspect("Events")]
    //[AuthorizeAspect("Admin")]
    public async Task<VideoEducationResponseDto> AddAsync(CreateVideoEducationRequestDto dto)
    {
        await _businessRules.VideoEducationTitleMustBeUnique(dto.Title);

        var videoEducation = _mapper.Map<VideoEducation>(dto);
       

        var addedVideoEducation = await _videoEducationRepository.AddAsync(videoEducation);
        return _mapper.Map<VideoEducationResponseDto>(addedVideoEducation);
    }


    //[LoggerAspect]
    //[ClearCacheAspect("Events")]
    //[AuthorizeAspect("Admin")]
    public async Task<string> DeleteAsync(int Id, bool permanent = false)
    {
        var videoEducation = await _businessRules.VideoEducationMustExist(Id);
        await _videoEducationRepository.DeleteAsync(videoEducation, permanent);
        return VideoEducationMessage.VideoEducationDeleted;
    }

    public Task<VideoEducation?> GetAsync(Expression<Func<VideoEducation, bool>> predicate, bool include = false, bool withDeleted = false, bool enableTracking = true, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<List<VideoEducation>> GetListAsync(Expression<Func<VideoEducation, bool>>? predicate = null, Func<IQueryable<VideoEducation>, IOrderedQueryable<VideoEducation>>? orderBy = null, bool include = false, bool withDeleted = false, bool enableTracking = true, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<Paginate<VideoEducation>> GetPaginateAsync(Expression<Func<VideoEducation, bool>>? predicate = null, Func<IQueryable<VideoEducation>, IOrderedQueryable<VideoEducation>>? orderBy = null, bool include = false, int index = 0, int size = 10, bool withDeleted = false, bool enableTracking = true, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    //[LoggerAspect]
    //[ClearCacheAspect("Events")]
    //[AuthorizeAspect("Admin")]
    public async Task<VideoEducationResponseDto> UpdateAsync(int Id , UpdateVideoEducationRequestDto dto)
    {
        var videoEducation = await _businessRules.VideoEducationMustExist(Id);

        _mapper.Map(dto, videoEducation);

        var updatedVideoEducation = await _videoEducationRepository.UpdateAsync(videoEducation);
        return _mapper.Map<VideoEducationResponseDto>(updatedVideoEducation);
    }
}
