

using AutoMapper;
using TechCareer.Models.Dtos.VideoEducation;
using TechCareer.Models.Entities;

namespace TechCareer.Service.Mappers.video;

public class VideoEducationMapper : Profile
{
    public VideoEducationMapper()
    {
        CreateMap<CreateVideoEducationRequestDto, videoEducation>();
        CreateMap<videoEducation, VideoEducationResponseDto>();
        CreateMap<UpdateVideoEducationRequestDto, videoEducation>();
    }
}
