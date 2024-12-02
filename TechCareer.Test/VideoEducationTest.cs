using AutoMapper;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TechCareer.DataAccess.Repositories.Abstracts;
using TechCareer.Models.Dtos.VideoEducation;
using TechCareer.Models.Entities;
using TechCareer.Models.Entities.Enum;
using TechCareer.Service.Abstracts;
using TechCareer.Service.Rules;
using Xunit;

public class VideoEducationServiceTests
{
    private readonly Mock<IVideoEducationRepository> _mockRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<VideoEducationBusinessRules> _mockBusinessRules;
    private readonly IVideoEducationService _service;

    public VideoEducationServiceTests()
    {
        _mockRepository = new Mock<IVideoEducationRepository>();
        _mockMapper = new Mock<IMapper>();
        _mockBusinessRules = new Mock<VideoEducationBusinessRules>();
        _service = new VideoEducationService(_mockRepository.Object, _mockMapper.Object, _mockBusinessRules.Object);
    }

    [Fact]
    public async Task AddAsync_Should_Add_VideoEducation_Successfully()
    {
        // Arrange
        var createDto = new CreateVideoEducationRequestDto
        {
            Title = "Test Title",
            Description = "Test Description",
            TotalHour = 10,
            IsCertified = true,
            Level = Level.Beginner,
            ImageUrl = "https://example.com/image.png",
            InstrutorId = Guid.NewGuid(),
            ProgrammingLanguage = "C#"
        };

        var videoEducation = new VideoEducation
        {
            Id = 1,
            Title = createDto.Title,
            Description = createDto.Description,
            TotalHour = createDto.TotalHour,
            IsCertified = createDto.IsCertified,
            Level = createDto.Level,
            ImageUrl = createDto.ImageUrl,
            InstrutorId = createDto.InstrutorId,
            ProgrammingLanguage = createDto.ProgrammingLanguage
        };

        _mockBusinessRules
            .Setup(r => r.VideoEducationTitleMustBeUnique(It.IsAny<string>()))
            .Returns(Task.CompletedTask);

        _mockRepository
            .Setup(r => r.AddAsync(It.IsAny<VideoEducation>()))
            .ReturnsAsync(videoEducation);

        _mockMapper
            .Setup(m => m.Map<VideoEducation>(createDto))
            .Returns(videoEducation);

        _mockMapper
            .Setup(m => m.Map<VideoEducationResponseDto>(videoEducation))
            .Returns(new VideoEducationResponseDto
            {
                Id = videoEducation.Id,
                Title = videoEducation.Title,
                Description = videoEducation.Description,
                TotalHour = videoEducation.TotalHour,
                Level = videoEducation.Level,
                ImageUrl = videoEducation.ImageUrl,
                InstrutorId = videoEducation.InstrutorId,
                ProgrammingLanguage = videoEducation.ProgrammingLanguage
            });

        // Act
        var result = await _service.AddAsync(createDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(videoEducation.Id, result.Id);
        Assert.Equal(videoEducation.Title, result.Title);
        _mockRepository.Verify(r => r.AddAsync(It.IsAny<VideoEducation>()), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_Should_Delete_VideoEducation_Successfully()
    {
        // Arrange
        var id = 1;
        var videoEducation = new VideoEducation { Id = id };

        _mockBusinessRules
            .Setup(r => r.VideoEducationMustExist(id))
            .ReturnsAsync(videoEducation);

        _mockRepository
            .Setup(r => r.DeleteAsync(videoEducation, It.IsAny<bool>()))
            .ReturnsAsync(videoEducation);

        // Act
        var result = await _service.DeleteAsync(id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Video education successfully deleted.", result);
        _mockRepository.Verify(r => r.DeleteAsync(videoEducation, It.IsAny<bool>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_Should_Update_VideoEducation_Successfully()
    {
        // Arrange
        var id = 1;
        var updateDto = new UpdateVideoEducationRequestDto
        {
            Id = id,
            Title = "Updated Title",
            Description = "Updated Description",
            InstructorId = Guid.NewGuid(),
            TotalHour = 20
        };

        var videoEducation = new VideoEducation
        {
            Id = id,
            Title = "Old Title",
            Description = "Old Description",
            TotalHour = 15
        };

        _mockBusinessRules
            .Setup(r => r.VideoEducationMustExist(id))
            .ReturnsAsync(videoEducation);

        _mockMapper
            .Setup(m => m.Map(updateDto, videoEducation))
            .Callback(() =>
            {
                videoEducation.Title = updateDto.Title;
                videoEducation.Description = updateDto.Description;
                videoEducation.TotalHour = updateDto.TotalHour;
            });

        _mockRepository
            .Setup(r => r.UpdateAsync(videoEducation))
            .ReturnsAsync(videoEducation);

        _mockMapper
            .Setup(m => m.Map<VideoEducationResponseDto>(videoEducation))
            .Returns(new VideoEducationResponseDto
            {
                Id = videoEducation.Id,
                Title = updateDto.Title,
                Description = updateDto.Description,
                TotalHour = updateDto.TotalHour
            });

        // Act
        var result = await _service.UpdateAsync(id, updateDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(updateDto.Title, result.Title);
        _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<VideoEducation>()), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_Should_Return_VideoEducation()
    {
        // Arrange
        var id = 1;
        var videoEducation = new VideoEducation { Id = id, Title = "Test Title" };

        _mockBusinessRules
            .Setup(r => r.VideoEducationMustExist(id))
            .ReturnsAsync(videoEducation);

        _mockMapper
            .Setup(m => m.Map<VideoEducationResponseDto>(videoEducation))
            .Returns(new VideoEducationResponseDto
            {
                Id = id,
                Title = videoEducation.Title,
                Description = videoEducation.Description
            });

        // Act
        var result = await _service.GetByIdAsync(id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(id, result.Id);
        Assert.Equal("Test Title", result.Title);
    }
}
