using AutoMapper;
using Moq;
using System.Linq.Expressions;
using TechCareer.DataAccess.Repositories.Abstracts;
using TechCareer.Models.Entities;
using TechCareer.Models.Events;
using TechCareer.Service.Concretes;
using TechCareer.Service.Constants;
using TechCareer.Service.Rules;

namespace EventTest
{
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
            // Arrange
            var createDto = new CreateEventRequestDto { Title = "Test Event" };
            var eventEntity = new Event { Id = Guid.NewGuid(), Title = "Test Event" };
            var eventResponse = new EventResponseDto { id = eventEntity.Id, Title = eventEntity.Title };

            _businessRulesMock.Setup(b => b.EventTitleMustBeUnique(It.IsAny<string>())).Returns(Task.CompletedTask);
            _mapperMock.Setup(m => m.Map<Event>(createDto)).Returns(eventEntity);
            _eventRepositoryMock.Setup(r => r.AddAsync(eventEntity)).ReturnsAsync(eventEntity);
            _mapperMock.Setup(m => m.Map<EventResponseDto>(eventEntity)).Returns(eventResponse);

            // Act
            var result = await _eventService.AddAsync(createDto);

            // Assert
            Assert.AreEqual(eventResponse.Title, result.Title);
            _businessRulesMock.Verify(b => b.EventTitleMustBeUnique(createDto.Title), Times.Once);
            _eventRepositoryMock.Verify(r => r.AddAsync(eventEntity), Times.Once);
        }

        [Test]
        public async Task DeleteAsync_ShouldDeleteEventSuccessfully()
        {
            // Arrange
            var eventId = Guid.NewGuid();
            var eventEntity = new Event { Id = eventId, Title = "Test Event" };

            _businessRulesMock.Setup(b => b.EventMustExist(eventId)).ReturnsAsync(eventEntity);
            _eventRepositoryMock.Setup(r => r.DeleteAsync(eventEntity, false)).Returns((Task<Event>)Task.CompletedTask);

            // Act
            var result = await _eventService.DeleteAsync(eventId);

            // Assert
            Assert.AreEqual(EventMessages.EventDeleted, result);
            _businessRulesMock.Verify(b => b.EventMustExist(eventId), Times.Once);
            _eventRepositoryMock.Verify(r => r.DeleteAsync(eventEntity, false), Times.Once);
        }

        [Test]
        public async Task GetByIdAsync_ShouldReturnEvent()
        {
            // Arrange
            var eventId = Guid.NewGuid();
            var eventEntity = new Event { Id = eventId, Title = "Test Event" };
            var eventResponse = new EventResponseDto { id = eventId, Title = "Test Event" };

            _businessRulesMock.Setup(b => b.EventMustExist(eventId)).ReturnsAsync(eventEntity);
            _mapperMock.Setup(m => m.Map<EventResponseDto>(eventEntity)).Returns(eventResponse);

            // Act
            var result = await _eventService.GetByIdAsync(eventId);

            // Assert
            Assert.AreEqual(eventResponse.id, result.id);
            Assert.AreEqual(eventResponse.Title, result.Title);
        }

        [Test]
        public async Task GetListAsync_ShouldReturnEventList()
        {
            // Arrange
            var eventList = new List<Event>
            {
                new Event { Id = Guid.NewGuid(), Title = "Event 1" },
                new Event { Id = Guid.NewGuid(), Title = "Event 2" }
            };

            var eventResponseList = new List<EventResponseDto>
            {
                new EventResponseDto { id = eventList[0].Id, Title = "Event 1" },
                new EventResponseDto { id = eventList[1].Id, Title = "Event 2" }
            };

            _eventRepositoryMock.Setup(r => r.GetListAsync(It.IsAny<Expression<Func<Event, bool>>>(), null, false, false, true, default))
                .ReturnsAsync(eventList);
            _mapperMock.Setup(m => m.Map<List<EventResponseDto>>(eventList)).Returns(eventResponseList);

            // Act
            var result = await _eventService.GetListAsync();

            // Assert
            Assert.AreEqual(2, result.Count);
        }
    }
}