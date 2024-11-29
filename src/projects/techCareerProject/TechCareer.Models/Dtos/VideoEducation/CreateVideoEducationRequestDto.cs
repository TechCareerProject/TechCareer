using TechCareer.Models.Entities.Enum;

namespace TechCareer.Models.Dtos.VideoEducation;

public sealed record CreateVideoEducationRequestDto(
    string Title, 
    string Description,
    double TotalHour,
    bool IsCertified,
    Level Level,
    string ImageUrl,
    Guid InstrutorId,
    string ProgrammingLanguage
);
