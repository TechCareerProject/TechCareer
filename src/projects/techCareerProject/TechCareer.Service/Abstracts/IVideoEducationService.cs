using Core.Persistence.Extensions;
using Core.Security.Entities;
using System.Linq.Expressions;
using TechCareer.Models.Dtos.VideoEducation;
using TechCareer.Models.Entities;

namespace TechCareer.Service.Abstracts;

public interface IVideoEducationService
{
    Task<VideoEducation?> GetAsync(
        Expression<Func<VideoEducation, bool>> predicate,
        bool include = false,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    );


    Task<Paginate<VideoEducation>> GetPaginateAsync(Expression<Func<VideoEducation, bool>>? predicate = null,
        Func<IQueryable<VideoEducation>, IOrderedQueryable<VideoEducation>>? orderBy = null,
        bool include = false,
        int index = 0,
        int size = 10,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default);


    Task<List<VideoEducation>> GetListAsync(Expression<Func<VideoEducation, bool>>? predicate = null,
        Func<IQueryable<VideoEducation>, IOrderedQueryable<VideoEducation>>? orderBy = null,
        bool include = false,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default);


    Task<VideoEducation> AddAsync(VideoEducation video);
    Task<VideoEducationResponseDto> UpdateAsync(UpdateVideoEducationRequestDto dto);
    Task<string> DeleteAsync(int Id, bool permanent = false);
}
