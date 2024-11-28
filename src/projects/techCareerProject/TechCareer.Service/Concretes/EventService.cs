using AutoMapper;
using Core.AOP.Aspects;
using TechCareer.DataAccess.Repositories.Abstracts;
using TechCareer.Models.Entities;
using TechCareer.Models.Events;
using TechCareer.Service.Abstracts;
using TechCareer.Service.Constants;
using TechCareer.Service.Rules;

namespace TechCareer.Service.Concretes
{
    public class EventService(
        IEventRepository eventRepository,
        IMapper mapper,
        EventBusinessRules businessRules
    ) : IEventService
    {
        [LoggerAspect]
        [ClearCacheAspect("Events")]
        [AuthorizeAspect("Admin")]
        public async Task<EventResponseDto> AddAsync(CreateEventRequestDto dto)
        {
            await businessRules.EventTitleMustBeUnique(dto.Title);

            var eventEntity = mapper.Map<Event>(dto);
            eventEntity.Id = Guid.NewGuid();

            var addedEvent = await eventRepository.AddAsync(eventEntity);

            return mapper.Map<EventResponseDto>(addedEvent);   
        }

        [LoggerAspect]
        [ClearCacheAspect("Events")]
        [AuthorizeAspect("Admin")]
        public async Task<string> DeleteAsync(Guid id)
        {
            var eventEntity = await businessRules.EventMustExist(id);   
            await eventRepository.DeleteAsync(eventEntity);
            return EventMessages.EventDeleted;
        }

        [CacheAspect(cacheKeyTemplate: "EventList", bypassCache: false, cacheGroupKey: "Events")]
        public async Task<List<EventResponseDto>> GetAllAsync()
        {
            var events = await eventRepository.GetListAsync(include: true, enableTracking: false);
            return mapper.Map<List<EventResponseDto>>(events);
        }

        public async Task<EventResponseDto> GetByIdAsync(Guid id)
        {
            var eventEntity = await businessRules.EventMustExist(id);
            return mapper.Map<EventResponseDto>(eventEntity);
        }

        [LoggerAspect]
        [ClearCacheAspect("Events")]
        [AuthorizeAspect("Admin")]
        public async Task<EventResponseDto> updateAsync(Guid id, UpdateEventRequestDto dto)
        {
            var eventEntity = await businessRules.EventMustExist(id);

            mapper.Map(dto, eventEntity);

            var updatedEvent = await eventRepository.UpdateAsync(eventEntity);
            return mapper.Map<EventResponseDto>(updatedEvent);
        }

        
    }
}
