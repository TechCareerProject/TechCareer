using AutoMapper;
using Moq;
using TechCareer.DataAccess.Repositories.Abstracts;
using TechCareer.Models.Entities;
using TechCareer.Models.Events;
using TechCareer.Service.Concretes;
using TechCareer.Service.Constants;
using TechCareer.Service.Rules;

[TestFixture]
public class EventServiceTests
{
    private Mock<IEventRepository> _eventRepositoryMock;
    private Mock<IMapper> _mapperMock;
    private Mock<EventBusinessRules> _businessRulesMock;
    private EventService _eventService;

    [SetUp]
    public void SetUp()
    {
        _eventRepositoryMock = new Mock<IEventRepository>();
        _mapperMock = new Mock<IMapper>();
        _businessRulesMock = new Mock<EventBusinessRules>();
        _eventService = new EventService(_eventRepositoryMock.Object, _mapperMock.Object, _businessRulesMock.Object);
    }

    [Test]
    public async Task AddAsync_ShouldAddEventSuccessfully()
    {
        var createDto = new CreateEventRequestDto(
            "Test Event",
            "Test Description",
            "TestImage.jpg",
            "Participation Text",
            1
        );

        var eventEntity = new Event
        {
            Id = Guid.NewGuid(),
            Title = "Test Event",
            Description = "Test Description",
            ImageUrl = "TestImage.jpg",
            ParticipationText = "Participation Text"
        };

        var eventResponse = new EventResponseDto
        {
            id = eventEntity.Id,
            Title = eventEntity.Title,
            Description = eventEntity.Description,
            ImageUrl = eventEntity.ImageUrl,
            ParticipationText = eventEntity.ParticipationText,
            CategoryName = "Test Category"
        };

        _businessRulesMock.Setup(b => b.EventTitleMustBeUnique(It.IsAny<string>())).Returns(Task.CompletedTask);
        _mapperMock.Setup(m => m.Map<Event>(createDto)).Returns(eventEntity);
        _eventRepositoryMock.Setup(r => r.AddAsync(eventEntity)).ReturnsAsync(eventEntity);
        _mapperMock.Setup(m => m.Map<EventResponseDto>(eventEntity)).Returns(eventResponse);

        var result = await _eventService.AddAsync(createDto);

        Assert.AreEqual(eventResponse.Title, result.Title);
        Assert.AreEqual(eventResponse.Description, result.Description);
        Assert.AreEqual(eventResponse.ImageUrl, result.ImageUrl);
        Assert.AreEqual(eventResponse.ParticipationText, result.ParticipationText);
        Assert.AreEqual(eventResponse.CategoryName, result.CategoryName);
    }

    [Test]
    public async Task DeleteAsync_ShouldDeleteEventSuccessfully()
    {
        var eventId = Guid.NewGuid();
        var eventEntity = new Event
        {
            Id = eventId,
            Title = "Test Event"
        };

        _businessRulesMock.Setup(b => b.EventMustExist(eventId)).ReturnsAsync(eventEntity);
        _eventRepositoryMock.Setup(r => r.DeleteAsync(eventEntity, false)).Returns((Task<Event>)Task.CompletedTask);

        var result = await _eventService.DeleteAsync(eventId);

        Assert.AreEqual(EventMessages.EventDeleted, result);
    }

    [Test]
    public async Task GetByIdAsync_ShouldReturnEvent()
    {
        var eventId = Guid.NewGuid();
        var eventEntity = new Event
        {
            Id = eventId,
            Title = "Test Event",
            Description = "Test Description",
            ImageUrl = "TestImage.jpg",
            ParticipationText = "Participation Text"
        };

        var eventResponse = new EventResponseDto
        {
            id = eventId,
            Title = "Test Event",
            Description = "Test Description",
            ImageUrl = "TestImage.jpg",
            ParticipationText = "Participation Text",
            CategoryName = "Test Category"
        };

        _businessRulesMock.Setup(b => b.EventMustExist(eventId)).ReturnsAsync(eventEntity);
        _mapperMock.Setup(m => m.Map<EventResponseDto>(eventEntity)).Returns(eventResponse);

        var result = await _eventService.GetByIdAsync(eventId);

        Assert.AreEqual(eventResponse.id, result.id);
        Assert.AreEqual(eventResponse.Title, result.Title);
        Assert.AreEqual(eventResponse.Description, result.Description);
        Assert.AreEqual(eventResponse.ImageUrl, result.ImageUrl);
        Assert.AreEqual(eventResponse.ParticipationText, result.ParticipationText);
    }
}
