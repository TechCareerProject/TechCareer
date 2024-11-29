

namespace TechCareer.Models.Dtos.VideoEducation;

public sealed record UpdateVideoEducationRequestDto
(
    int Id,
    string Title,
    string Description,
    Guid InstrutorId
);
